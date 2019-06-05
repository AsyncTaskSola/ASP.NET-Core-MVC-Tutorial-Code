using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Heavy.Web.Filters
{
    public class LogResourceFilter: Attribute,IResourceFilter
    {
        //授权之后执行
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine("Executing Resource Filter!");
        }
        //所有管道执行完后再执行

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine("Executed Resource Filter!");
        }
    }
}
