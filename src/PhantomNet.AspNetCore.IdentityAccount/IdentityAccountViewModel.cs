﻿using System.Collections.Generic;

namespace PhantomNet.AspNetCore.IdentityAccount
{
    public class IdentityAccountViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
