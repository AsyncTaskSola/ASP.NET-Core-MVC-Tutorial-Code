using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Heavy.Web.Auth
{
    public class AdiminstratorsHandler:AuthorizationHandler<QualifiedUserRequirment>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            QualifiedUserRequirment requirement)
        {
            if (context.User.IsInRole("Administrators"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
