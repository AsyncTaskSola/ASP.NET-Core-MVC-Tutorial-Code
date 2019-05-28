
using System.Threading.Tasks;
using Heavy.Web.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Heavy.Web.Auth
{
    public class EmailHandler:AuthorizationHandler<EmailRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            EmailRequirement requirement)
        {
            var claim = context.User.FindFirst(x => x.Type == "Email");
            if (claim != null)
            {
                if (claim.Value.EndsWith(requirement.RequiredEmail))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;//当前线程结果什么没做
        }
    }
}
