using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_S.Baskets;
using ECommerce.Application.Specifications;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentGateway paymentGateway;
        private readonly PaymentGatewaySettings _stripe;
        private readonly IMapper mapper;

        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IPaymentGateway paymentGateway, IOptions<PaymentGatewaySettings> stripeSettings, IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.paymentGateway = paymentGateway;
            this.mapper = mapper;
            _stripe = stripeSettings.Value;
        }
        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default)
        {
            // 1. Check Basket and Items
            var basket = await basketRepository.GetBasketAsync(basketId, ct);

            if (basket is null) return Result<BasketDto>.fail(Error.NotFound("Basket Not Found", $"Basket With Id {basketId} not Found"));

            if (basket.Items.Count == 0) return Result<BasketDto>.fail(Error.validation("Basket Is Empty", $"Basket With Id {basketId} Is Empty"));
            //-------------------------------------------------------------------------------------------------------------------------
            // 2. Check Product Is Exsist or not
            var productRepo = unitOfWork.GetRepository<Product, int>();

            var ProductIds = basket.Items.Select(i => i.Id).ToHashSet();

            var products = await productRepo.GetAllWithSpecificationsAsync(new ProductWithIdsSpecifications(ProductIds),ct);

            foreach (var item in basket.Items)
            {
                var product = products.FirstOrDefault(P => P.Id == item.Id);

                if (product is null) return Result<BasketDto>.fail(Error.NotFound("Product Not Found", $"Product With Id {item.Id} Is Empty"));

                item.Price = product.Price;
            }
            var deliveryRepo = unitOfWork.GetRepository<DeliveryMethod, int>();

            var DeliveryMethod = await deliveryRepo.GetByIdAsync(basket.DeliveryMethodId.Value, ct);

            if (DeliveryMethod is null) return Result<BasketDto>.fail(Error.NotFound("Delivery Mehtod Not Found", $"Delivery Mehtod With Id {basket.DeliveryMethodId.Value} Is Empty"));

            basket.ShippingPrice = DeliveryMethod.Cost;

            var subTotal = basket.Items.Sum(i => i.Quantity * i.Price);

            var amount = (long)Math.Round((subTotal + DeliveryMethod.Cost) * 100m);

            if (!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var result = await paymentGateway.UpdatePaymentIntentAysnc(basket.PaymentIntentId, amount, ct);
                basket.PaymentIntentId = result.PaymentIntentId;
                basket.ClientSecret = result.ClientSecret;
            }
            else
            {
                var result = await paymentGateway.CreatePaymentIntentAsync(amount, _stripe.DefaultCurrency, ct);
                basket.PaymentIntentId = result.PaymentIntentId;
                basket.ClientSecret = result.ClientSecret;
            }
            await basketRepository.CreateOrUpdateBasketAsync(basket, ct: ct);

            return Result<BasketDto>.Ok(mapper.Map<BasketDto>(basket));
        }
    }
    public class PaymentGatewaySettings
    {
        public string SecretKey { get; set; } = default!;

        public string DefaultCurrency { get; set; } = "USD";
    }
}
