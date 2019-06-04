﻿using System.Diagnostics;
using Heavy.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Heavy.Web.Models;
using Microsoft.Extensions.Logging;

namespace Heavy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController>logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            //throw  new Exception("Someting happened");   app.UseStatusCodePages();

            _logger.LogInformation(MyLogEventIds.Homepage,"Visting Home Index..");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult MyError()
        {
            return View();
        }
    }
}
