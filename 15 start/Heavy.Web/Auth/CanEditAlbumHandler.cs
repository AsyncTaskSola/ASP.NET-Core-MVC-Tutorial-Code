using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Heavy.Web.Auth
{
    public class CanEditAlbumHandler:AuthorizationHandler<QualifiedUserRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, QualifiedUserRequirment requirement)
        {
            if (context.User.HasClaim(x => x.Type == "Edit Album"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
