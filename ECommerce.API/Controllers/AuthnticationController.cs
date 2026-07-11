using ECommerce.Application.Contracts;
using ECommerce.Application.DTO_S.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class AuthnticationController : ApiBaseController
    {
        private readonly IAuthnticationServices authenticationServices;

        public AuthnticationController(IAuthnticationServices authenticationServices)
        {
            this.authenticationServices = authenticationServices;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.LoginAsync(loginDto, ct));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.RegisterAsync(registerDto, ct));
        }

        [HttpGet("emailexist")]
        public async Task<ActionResult<bool>> CheckEmail([FromQuery] string email , CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.CheckEmailAsync(email, ct)) ;
        }
        [HttpGet("CurrentUser")]
        [Authorize]

        public async Task<ActionResult<UserDto>> GetCurrentuser([FromQuery] string email, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.GetCurrentUser(email, ct));
        }
        [HttpGet("address")]
        [Authorize]

        public async Task<ActionResult<AddressDto>> GetUserAddress([FromQuery] string email, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.GetUserAddressAsync(email, ct));
        }
        [HttpPost("address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress([FromQuery] string email,[FromBody]AddressDto addressDto, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.UpdateAddressAsync(addressDto,email, ct));
        }

    }
}
