using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Heavy.Web.ViewComponents
{
    public class InternetStatus:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var httpClient = new HttpClient();
            var reponse = await httpClient.GetAsync("https://www.baidu.com");
            if (reponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return View(true);
            }
            return View(false);
        }
    }
}
