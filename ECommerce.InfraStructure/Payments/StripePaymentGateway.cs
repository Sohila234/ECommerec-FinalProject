using ECommerce.Application.Services;
using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Payments
{
    public class StripePaymentGateway : IPaymentGateway
    {
        private readonly PaymentIntentService _paymentIntentService = new();

        public StripePaymentGateway(IOptions<PaymentGatewaySettings> options)
        {
            StripeConfiguration.ApiKey = options.Value.SecretKey;
        }

        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default)
        {
            var IntentOptions = new PaymentIntentCreateOptions
            {
                Amount = (long)amount,
                Currency = currency.ToLower(),
                PaymentMethodTypes = ["card"]
            };

            var intent = await _paymentIntentService.CreateAsync(IntentOptions, cancellationToken: ct);

            return new PaymentIntentResult(intent.Id, intent.ClientSecret);
        }

        public async Task<PaymentIntentResult> UpdatePaymentIntentAysnc(string PaymentIntentId, decimal amount, CancellationToken ct = default)
        {
            var IntentOptions = new PaymentIntentUpdateOptions
            {
                Amount = (long)amount,
            };

            var intent = await _paymentIntentService.UpdateAsync(PaymentIntentId, IntentOptions, cancellationToken: ct);

            return new PaymentIntentResult(intent.Id, intent.ClientSecret);
        }
    }

   
}
