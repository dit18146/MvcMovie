using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    [Route("kendo")]
    public class KendoTestController : Controller
    {
        [Route("test", Name = "Kendo_Test"), HttpGet]
        public IActionResult Test()
        {
            return View();
        }

        [Route("kendo-test/read", Name = "KendoGrid_Test_Read")]
        public JsonResult TestKendoGridRead ([DataSourceRequest] DataSourceRequest request)
        {
            var model = new List<TestMovie>
            {
                new TestMovie{Id = 1, Title = "The Lord of the Rings", Description = "Description" },
                new TestMovie{Id = 2, Title = "BladeRunner", Description = "Description" }
            };

            return Json(model.ToDataSourceResult(request));
        }
    }
}
