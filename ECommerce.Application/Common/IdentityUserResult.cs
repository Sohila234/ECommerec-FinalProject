using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Common
{
    public class IdentityUserResult
    {
        public IdentityUserResult(string id, string email, string displayName, string userName)
        {
            Id = id;
            Email = email;
            DisplayName = displayName;
            UserName = userName;
        }

        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string UserName { get; set; } = default!;
    }
}
