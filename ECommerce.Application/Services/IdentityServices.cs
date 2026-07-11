using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_S.Identity;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace ECommerce.Application.Services
{
    public class IdentityServices : IIdentityServices
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IdentityServices(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<bool>> CheckPasswordAsync(string password, string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) return Result<bool>.fail(Error.NotFound("User Not Found."));

            var isValid = await userManager.CheckPasswordAsync(user, password);
            return Result<bool>.Ok(isValid);
        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.UserName,
                DisplayName = registerDto.DisplayName,
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
                return Result<IdentityUserResult>.fail(errors);
            }
            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.Email, user.DisplayName, user.UserName));
        }


        public async Task<Result<IdentityUserResult>> FindByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) return Result<IdentityUserResult>.fail(Error.NotFound("user not found."));

            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.Email, user.DisplayName, user.UserName));
        }

       
        public async Task<Result<IEnumerable<string>>> GetRolesAsync(string Email)
        {
            var User = await userManager.FindByEmailAsync(Email);
            if(User is null)
                return Result<IEnumerable<string>>.fail(Error.NotFound("user is not found "));
            var roles = await userManager.GetRolesAsync(User);
            return Result<IEnumerable<string>>.Ok(roles);

        }
        public async Task<Result<AddressDto>> GetAddressBtEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email, ct);
            if (user is null) return Result<AddressDto>.fail(Error.NotFound("user is not found "));
            if(user.Address is null) return Result<AddressDto>.fail(Error.NotFound("address is not found "));
            return Result<AddressDto>.Ok(new AddressDto()
            {
                FirstName = user.Address.FirstName,
                LastName = user.Address.LastName,
                Street = user.Address.Street,
                City = user.Address.City,
                Country = user.Address.Country,
            });

        }


        public async Task<Result<AddressDto>> UpdateAddressAsync(string email, AddressDto addressDto, CancellationToken ct = default)
        {
            var user = await userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email, ct);
            if (user is null) return Result<AddressDto>.fail(Error.NotFound("user is not found "));
            if (user.Address is null)
            {
                user.Address = new Address()
                {
                    FirstName = addressDto.FirstName,
                    LastName= addressDto.LastName,
                    City = addressDto.City,
                    Country= addressDto.Country,
                    Street= addressDto.Street,

                };
            }
            else
            {
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;
                user.Address.Street = addressDto.Street;
            }
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result<AddressDto>.fail(Error.Failure("can not update address "));
            return Result<AddressDto>.Ok(addressDto);
        }

        public async Task<Result<bool>> EmailExistAsync(string email, CancellationToken ct = default)
        {
            return await userManager.FindByEmailAsync(email) is not null;
        }
    }
}
