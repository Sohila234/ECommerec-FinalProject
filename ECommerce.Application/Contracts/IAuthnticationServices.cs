using ECommerce.Application.Common;
using ECommerce.Application.DTO_S.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Contracts
{
    public interface IAuthnticationServices
    {
        Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default);
        Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default);
        Task <Result<bool>> CheckEmailAsync (string email, CancellationToken ct = default);
        Task<Result<AddressDto>> GetUserAddressAsync (string email, CancellationToken ct = default);
        Task<Result<AddressDto>> UpdateAddressAsync (AddressDto addressDto, string email, CancellationToken ct = default);
        Task<Result<UserDto>> GetCurrentUser (string email, CancellationToken ct = default);


    }
}
