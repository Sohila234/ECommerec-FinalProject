using ECommerce.Application.Common;
using ECommerce.Application.DTO_S.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Contracts
{
    public interface IIdentityServices
    {
        Task<Result<IdentityUserResult>> FindByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<bool>> CheckPasswordAsync(string password, string email, CancellationToken ct = default);
        Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto, CancellationToken ct = default);
        Task<Result<IEnumerable<string>>> GetRolesAsync(string Email);
        Task<Result<AddressDto>> GetAddressBtEmailAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> UpdateAddressAsync (string email ,AddressDto addressDto ,CancellationToken ct = default );
        Task<Result<bool>> EmailExistAsync(string email, CancellationToken ct = default);



    }
}
