using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTO_S.Identity
{
    public class UserDto
    {
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
