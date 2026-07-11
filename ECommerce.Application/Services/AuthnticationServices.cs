using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_S.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services
{
    public class AuthnticationServices : IAuthnticationServices
    {
        private readonly IIdentityServices identityServices;
        private readonly ITokenServices tokenServices;

        public AuthnticationServices(IIdentityServices identityServices ,ITokenServices tokenServices)
        {
            this.identityServices = identityServices;
            this.tokenServices = tokenServices;
        }

       
        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            var userResult = await identityServices.FindByEmailAsync(loginDto.Email, ct);
            if (!userResult.IsSuccess) return Result<UserDto>.fail(userResult.Errors);

            var passwordCheck = await identityServices.CheckPasswordAsync(loginDto.Password, loginDto.Email, ct);
            if (!passwordCheck.IsSuccess) return Result<UserDto>.fail(Error.UnAuthorized("Invalid Email or Password."));

            var rolesResult = await identityServices.GetRolesAsync(userResult.Data.Email);
            var token = tokenServices.CreateToken(userResult.Data.Id, userResult.Data.Email, userResult.Data.UserName, rolesResult.Data);

            return Result<UserDto>.Ok(new UserDto
            {
                DisplayName = userResult.Data.DisplayName,
                Email = userResult.Data.Email,
                Token = token
            });
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var result = await identityServices.CreateUserAsync(registerDto, ct);
            if (!result.IsSuccess) return Result<UserDto>.fail(result.Errors);
            var rolesResult = await identityServices.GetRolesAsync(result.Data.Email);
            var token = tokenServices.CreateToken(result.Data.Id, result.Data.Email, result.Data.UserName, rolesResult.Data);

            return Result<UserDto>.Ok(new UserDto
            {
                DisplayName = result.Data.DisplayName,
                Email = result.Data.Email,
                Token = token
            });
        }
        public async Task<Result<bool>> CheckEmailAsync(string email, CancellationToken ct = default)
        {
            return await identityServices.EmailExistAsync(email, ct);
        }

        public async Task<Result<AddressDto>> GetUserAddressAsync(string email, CancellationToken ct = default)
        {

            var result = await identityServices.GetAddressBtEmailAsync(email, ct);
            if(!result.IsSuccess)
                return Result<AddressDto>.fail(result.Errors);
            return Result<AddressDto>.Ok(result.Data);

        }

        public async Task<Result<AddressDto>> UpdateAddressAsync(AddressDto addressDto, string email, CancellationToken ct = default)
        {
            return await identityServices.UpdateAddressAsync(email, addressDto, ct);
        }

        public async Task<Result<UserDto>> GetCurrentUser(string email, CancellationToken ct = default)
        {
           var UserResult = await identityServices.FindByEmailAsync(email, ct);
            if(!UserResult.IsSuccess)
                return Result<UserDto>.fail(UserResult.Errors);
            var user = UserResult.Data;
            var roleResult = await identityServices.GetRolesAsync(user.Email);
            if (!roleResult.IsSuccess)
                return Result<UserDto>.fail(roleResult.Errors);
            var token = tokenServices.CreateToken(user.Id , user.Email , user.UserName ,roleResult.Data);
            return Result<UserDto>.Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = token
            });

        }

    }
}
