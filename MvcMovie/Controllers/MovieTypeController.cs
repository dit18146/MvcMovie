using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers;

public class MovieTypeController : Controller
{
    [Route("categories")]
    public IActionResult Index() => View();
}