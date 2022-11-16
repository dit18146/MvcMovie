using Microsoft.AspNetCore.Mvc;
using MvcMovie.Data;
using MvcMovie.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Docs.Samples;
using MvcMovie.Services;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Xml.Linq;
using System.Data.Entity;

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

        [Route("", Name = "Movies_Index")]
        public IActionResult Index()
        {
            /*_movieService.Update(new Movie(2, "rocky"));

            _movieService.Delete(new Movie(5, "james bond"));

            _movieService.Add(new Movie(5, "james bond"));*/

            /*var model = _movieService.GetCollection();
            var item = new Movie(50, "The hunt for the red October");
            var item2 = new Movie(2, "James Bond");
            _movieService.Add(item);
            _movieService.Delete(_movieService.GetById(1));
            _movieService.Update(item2);*/


            _movieService.ClearDatabase();

            var model = _movieService.GetCollection();

            return View(model);
        }

        [Route("detail/{id:int?}", Name = "Movies_Details")] //naming is important        
        public IActionResult Details(int? id)
        {
            var model = _movieService.GetById(id);

            return View(model);
   
        }

        [Route("create", Name = "Create"), HttpGet]
        public IActionResult Create()
        {
            var model = new MovieViewModel();

            return View(model);
        }




        [Route("create", Name = "Create_Post"), HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MovieViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_movieService.CheckIfExists(model.Id) == false)
                {
                    _movieService.Add(new Movie(model.Id, model.Title));
                }
                else
                    ViewData["fail"] = "Id exists, please reenter id and movie title";

                TempData["success"] = "Created successfully";

                return RedirectToAction(nameof(Index));
            }

            ViewData["fail"] = "Wrong Input";

            return View(model);

        }


        [Route("update/{id:int?}", Name = "Update_Get"), HttpGet]     
        public IActionResult Update(int? id)
        {
            var model = _movieService.GetById(id);

            if (id != null)
            {
                if (_movieService.CheckIfExists((int)id))
                {
                    return View(model);
                }

            }

            else
            {
                id = 1;

                return View(model);
            }
                

            TempData["fail"] = "Wrong Input, model validation failed";

            return RedirectToAction(nameof(Index));

        }

        [Route("update", Name = "Update_Post"), HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(MovieViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                _movieService.Update(new Movie(model.Id, model.Title));

                TempData["success"] = "Upadated successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["fail"] = "Wrong Input, model validation failed";

            return RedirectToAction(nameof(Index));

        }



        [Route("delete/{id:int?}", Name = "Delete_Get"), HttpGet]
        public IActionResult Delete(int? id)
        {

            var model = _movieService.GetById(id);

            if (id != null)
            {

                if (_movieService.CheckIfExists((int)id))
                {
                    // var model = _movieService.GetById(id);

                    return View(model);
                }
            }

            else
            {
                id = 1;

                return View(model);
            }

            TempData["fail"] = "Wrong Input, model validation failed";

            return RedirectToAction(nameof(Index));

        }

        [Route("delete", Name = "Delete_Post"), HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(MovieViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_movieService.CheckIfExists(model.Id) == true)
                {
                    _movieService.Delete(new Movie(model.Id, model.Title));

                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["fail"] = "Wrong Input, model validation failed";

            return RedirectToAction(nameof(Index));
        }

    }
}
