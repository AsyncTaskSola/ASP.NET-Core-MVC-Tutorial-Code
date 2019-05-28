using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Heavy.Web.Auth
{
    /// <summary>
    /// 合适的用户请求,具体是怎么合格是handle来决定了
    /// </summary>
    public class QualifiedUserRequirment:IAuthorizationRequirement
    {

    }
}
