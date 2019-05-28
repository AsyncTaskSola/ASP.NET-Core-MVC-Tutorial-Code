using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Heavy.Web.Auth
{
    public class EmailRequirement:IAuthorizationRequirement
    {
        public string RequiredEmail { get; }

        public EmailRequirement(string requiredEmail)
        {
            RequiredEmail = requiredEmail;
        }
    }
}
