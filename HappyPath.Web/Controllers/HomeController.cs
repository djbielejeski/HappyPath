using HappyPath.Services.Domain;
using HappyPath.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HappyPath.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly ITestItemService _testItemService;
        public HomeController(ITestItemService testItemService)
        {
            _testItemService = testItemService;
        }

        public ActionResult Index()
        {
            var testItem = new TestItem { Name = "123", Value = 123 };
            _testItemService.AddOrUpdate(testItem);

            ViewBag.testItem = testItem;
            return View();
        }
    }
}