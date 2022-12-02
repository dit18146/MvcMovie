using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using MvcMovie.Services;

namespace MvcMovie.Controllers;

[Route("movies")]
//[Route("[controller]/[action]/[?id]")]
public class MoviesController : Controller
{
    private readonly IMovieService _movieService;

    private readonly IMovieTypeService _movieTypeService;


    public MoviesController(IMovieService movieService, IMovieTypeService movieTypeService)
    {
        _movieService = movieService;

        _movieTypeService = movieTypeService;
    }

    [Route("json", Name = "Movies_Json_Index")]
    public IActionResult Json_Index()
    {
        _movieService.ClearDatabase();

        _movieTypeService.ClearDatabase();

        var model = _movieService.GetCollection();

        return Json(model);
    }

    [Route("json-detail/{id:int?}", Name = "Movies_Json_Details")] //naming is important        
    public IActionResult Json_Details(int? id)
    {
        var model = _movieService.GetById(id);

        return Json(model);
    }

    [Route("json-create", Name = "Create_Json_Post"), HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Json_Create(MovieViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["fail"] = "Wrong Input";

            return Json(model);
        }

        if (_movieService.CheckIfExists(model.Id) == false)
        {
            if (model.Description == null)

                model.Description = "N/A";


            if (model.MovieTypeId == null)

                _movieService.Add(new Movie(model.Id, model.Title, model.Description, 10));

            else
                _movieService.Add(new Movie(model.Id, model.Title, model.Description, (int)model.MovieTypeId));
        }
        else

        {
            ViewData["fail"] = "Id exists, please reenter id and movie title";
        }

        TempData["success"] = "Created successfully";

        return Json(model);
    }


    [Route("json-update", Name = "Update_Json_Post"), HttpPost]
    public IActionResult Json_Update(MovieViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.MovieTypeId == null)

                _movieService.Update(new Movie(model.Id, model.Title, model.Description, 10));

            else

                _movieService.Update(new Movie(model.Id, model.Title, model.Description, (int)model.MovieTypeId));

            TempData["success"] = "Upadated successfully";

            return Json(model);
        }

        TempData["fail"] = "Wrong Input, model validation failed";

        return Json(model);
    }

    [Route("json-delete", Name = "Delete_Json_Post"), HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Json_Delete(MovieViewModel model)
    {
        if (ModelState.IsValid)
            if (_movieService.CheckIfExists(model.Id))
            {
                _movieService.Delete(new Movie(model.Id, model.Title, model.Description));

                TempData["success"] = "Upadated successfully";

                return Json(model);
            }

        TempData["fail"] = "Wrong Input, model validation failed";

        return Json(model);
    }

    [Route("Ajax", Name = "Movies_Ajax_Index")]
    public IActionResult Ajax_Index()
    {
        _movieService.ClearDatabase();

        _movieTypeService.ClearDatabase();

        var model = _movieService.GetCollection();

        return PartialView(model);
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

        _movieTypeService.ClearDatabase();

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
        _movieTypeService.ClearDatabase();

        var model = new MovieViewModel();

        model.Categories = _movieTypeService.GetCollection();

        return View(model);
    }


    [Route("create", Name = "Create_Post"), HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(MovieViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["fail"] = "Wrong Input";

            return View(model);
        }

        if (_movieService.CheckIfExists(model.Id) == false)
        {
            if (model.Description == null)

                model.Description = "N/A";


            if (model.MovieTypeId == null)

                _movieService.Add(new Movie(model.Id, model.Title, model.Description, 10));

            else
                _movieService.Add(new Movie(model.Id, model.Title, model.Description, (int)model.MovieTypeId));
        }
        else

        {
            ViewData["fail"] = "Id exists, please reenter id and movie title";
        }

        TempData["success"] = "Created successfully";

        return RedirectToAction(nameof(Index));
    }


    [Route("update/{id:int?}", Name = "Update_Get"), HttpGet]
    public IActionResult Update(int? id)
    {
        _movieTypeService.ClearDatabase();

        var model = _movieService.GetById(id);

        model.Categories = _movieTypeService.GetCollection();

        if (id != null)
        {
            if (_movieService.CheckIfExists((int)id)) return View(model);
        }

        else
        {
            id = 18;

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
            if (model.MovieTypeId == null)

                _movieService.Update(new Movie(model.Id, model.Title, model.Description, 10));

            else

                _movieService.Update(new Movie(model.Id, model.Title, model.Description, (int)model.MovieTypeId));

            TempData["success"] = "Upadated successfully";

            return RedirectToAction(nameof(Update), new { id = model.Id });
        }

        TempData["fail"] = "Wrong Input, model validation failed";

        return RedirectToAction(nameof(Update), new { id = model.Id });
    }


    [Route("delete/{id:int?}", Name = "Delete_Get"), HttpGet]
    public IActionResult Delete(int? id)
    {
        _movieTypeService.ClearDatabase();

        var model = _movieService.GetById(id);

        model.Categories = _movieTypeService.GetCollection();

        if (id != null)
        {
            if (_movieService.CheckIfExists((int)id))
                // var model = _movieService.GetById(id);
                return View(model);
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
            if (_movieService.CheckIfExists(model.Id))
            {
                _movieService.Delete(new Movie(model.Id, model.Title, model.Description));

                TempData["success"] = "Upadated successfully";

                return RedirectToAction(nameof(Index));
            }

        TempData["fail"] = "Wrong Input, model validation failed";

        return RedirectToAction(nameof(Index));
    }

    [Route("categories", Name = "Categories_Index")]
    public IActionResult Categories_Index()
    {
        _movieTypeService.ClearDatabase();

        var model = _movieTypeService.GetCollection();

        return View(model);
    }


    [Route("createCategory", Name = "Create_Category"), HttpGet]
    public IActionResult Create_Category()
    {
        var model = new MovieTypeViewModel();

        return View(model);
    }


    [Route("createCategory", Name = "Create_Category_Post"), HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create_Category(MovieTypeViewModel model)
    {
        if (ModelState.IsValid)
        {
            _movieTypeService.Add(new MovieType(model.Id, model.Name));

            TempData["success"] = "Created successfully";

            return RedirectToAction(nameof(Categories_Index));
        }

        ViewData["fail"] = "Wrong Input";

        return View(model);
    }

    [Route("updateCategory/{id:int?}", Name = "Update_Category_Get"), HttpGet]
    public IActionResult Update_Category(int? id)
    {
        var model = _movieTypeService.GetById(id);

        if (id != null)
        {
            if (_movieTypeService.CheckIfExists((int)id)) return View(model);
        }

        else
        {
            id = 1;

            return View(model);
        }


        TempData["fail"] = "Wrong Input, model validation failed";

        return RedirectToAction(nameof(Index));
    }

    [Route("updateCategory", Name = "Update_Category_Post"), HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(MovieTypeViewModel model)
    {
        if (ModelState.IsValid)
        {
            _movieTypeService.Update(new MovieType(model.Id, model.Name));

            TempData["success"] = "Upadated successfully";

            return RedirectToAction(nameof(Categories_Index));
        }

        TempData["fail"] = "Wrong Input, model validation failed";

        return RedirectToAction(nameof(Categories_Index));
    }
}