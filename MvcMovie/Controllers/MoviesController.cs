using Microsoft.AspNetCore.Mvc;
using MvcMovie.Data;
using MvcMovie.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Docs.Samples;
using MvcMovie.Services;

namespace MvcMovie.Controllers
{
    [Route("movies")]
    //[Route("[controller]/[action]/[?id]")]
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        
        
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;        
        }

        [Route("")]
        public IActionResult Index()
        {
            var model = _movieService.GetCollection();
            var item = new Movie(50, "The hunt for the red October");
            var item2 = new Movie(2, "James Bond");
            _movieService.Add(item);
            _movieService.Delete(_movieService.GetById(1));
            _movieService.Update(item2);
            return View(model);
        }

        [Route("detail/{id:int?}", Name = "Movies_Details")] //naming is important
        
        public IActionResult Details(int? id)
        {
            var model = _movieService.GetById(id);
            return View(model);
            //return ControllerContext.MyDisplayRouteInfo(id);
        }


    }
}
