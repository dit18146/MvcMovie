using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using MvcMovie.Services;
using System.Web;
using Newtonsoft.Json;
using CsvHelper;
using Bond.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        var model = _movieService.GetCollection();

        return Json(model);
    }

    [Route("json-detail/{id:int?}", Name = "Movies_Json_Details")]        
    public IActionResult Json_Details(int? id)
    {
        var model = _movieService.GetById(id);

        return Json(model);
    }

    [Route("json-create", Name = "Create_Json_Post"), HttpPost]
    public IActionResult Json_Create(MovieViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["fail"] = "Wrong Input";

            return Json(model);
        }

        if (_movieService.CheckIfExists(model.Id) == false)
        {
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
            _movieService.Update(new Movie(model.Id, model.Title, model.Description, (int)model.MovieTypeId));

            TempData["success"] = "Upadated successfully";

            return Json(model);
        }

        TempData["fail"] = "Wrong Input, model validation failed";

        return Json(model);
    }

    [Route("json-delete", Name = "Delete_Json_Post"), HttpPost]
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
        var model = _movieService.GetCollection();

        return PartialView(model);
    }

    [Route("", Name = "Movies_Index")]
    public IActionResult Index()
    {
        var model = _movieService.GetCollection();

        //var model = await _movieService.GetCollectionAsync().ConfigureAwait(false);


        return View(model);
    }


    [Route("detail/{id:int?}", Name = "Movies_Details")] //naming is important        
    public IActionResult Details(int? id)
    {
        var model = _movieService.GetById(id);

        return View(model);
    }

    [Route("detail-offcanvas/{id:int?}", Name = "Movies_Details_Offcanvas")] //naming is important        
    public IActionResult Details_Offcanvas(int? id)
    {
        var model = _movieService.GetById(id);

        return PartialView("_Details");
    }

    [Route("create", Name = "Create"), HttpGet]
    public IActionResult Create()
    {

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
            model.Categories = _movieTypeService.GetCollection();

            ViewData["fail"] = "Wrong Input";

            return View(model);
        }

        if (_movieService.CheckIfExists(model.Id) == false)

                _movieService.Add(new Movie(model.Id, model.Title, model.Description, (int)model.MovieTypeId));
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
       var model = _movieService.GetById(id);

        model.Categories = _movieTypeService.GetCollection();

        if (_movieService.CheckIfExists((int)id)) 

            return View(model);

        TempData["fail"] = "Wrong Input, model validation failed";

        return RedirectToAction(nameof(Index));
    }

    [Route("update", Name = "Update_Post"), HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(MovieViewModel model)
    {
        if (ModelState.IsValid)
        {
            _movieService.Update(new Movie(model.Id, model.Title, model.Description, (int)model.MovieTypeId));

            TempData["success"] = "Upadated successfully";

            return RedirectToAction(nameof(Update), new { id = model.Id });
        }

        TempData["fail"] = "Wrong Input, model validation failed";

        return View(new Movie(model.Id, model.Title, model.Description, (int)model.MovieTypeId));
    }


    [Route("delete/{id:int?}", Name = "Delete_Get"), HttpGet]
    public IActionResult Delete(int? id)
    {
        var model = _movieService.GetById(id);

        model.Categories = _movieTypeService.GetCollection();

      
        if (_movieService.CheckIfExists((int)id))
                
            return View(model);
        
        TempData["fail"] = "Wrong Input, model validation failed";

        return RedirectToAction(nameof(Index));
    }

    [Route("delete", Name = "Delete_Post"), HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(MovieViewModel model)
    {
       
        if (_movieService.CheckIfExists(model.Id))
        {
            _movieService.Delete(new Movie(model.Id, model.Title, model.Description));

            TempData["success"] = "Upadated successfully";

            return RedirectToAction(nameof(Index));
        }

        TempData["fail"] = "Wrong Input, model validation failed";

        return RedirectToAction(nameof(Index));
    }

    [Route("delete-htmx/{id:int?}", Name = "Delete_Htmx"), HttpDelete]
    public void Delete_Htmx(int? id)
    {

        var model = _movieService.GetById(id);

        model.Categories = _movieTypeService.GetCollection();


        if (_movieService.CheckIfExists((int)id))
        {
            _movieService.Delete(new Movie(model.Id, model.Title, model.Description));

            TempData["success"] = "Upadated successfully";
        }

        TempData["fail"] = "Wrong Input, model validation failed";

    }

    [Route("categories", Name = "Categories_Index")]
    public IActionResult Categories_Index()
    {
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

         if (_movieTypeService.CheckIfExists((int)id)) 

            return View(model);
       
        TempData["fail"] = "Wrong Input, model validation failed";

        return RedirectToAction(nameof(Index));
    }

    [Route("updateCategory", Name = "Update_Category_Post"), HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update_Category(MovieTypeViewModel model)
    {
        if (ModelState.IsValid)
        {
            _movieTypeService.Update(new MovieType(model.Id, model.Name));

            TempData["success"] = "Upadated successfully";

            return RedirectToAction(nameof(Categories_Index));
        }

        TempData["fail"] = "Wrong Input, model validation failed";

        return View(new MovieType(model.Id, model.Name));
    }

    [Route("Kendo", Name = "Kendo_Index")]
    public IActionResult Kendo_Index()
    {
        var model = _movieService.GetCollection();

        return View(model);
    }

    [Route("json-kendo", Name = "Kendo_Json_Index")]
    public IActionResult Kendo_Json([DataSourceRequest] DataSourceRequest request)
    {
        var model = _movieService.GetCollection();

        return Json(model.Items.ToDataSourceResult(request));
    }

    
   
    
    [Route("read-file", Name = "Read_File"), HttpPost]
    public ActionResult Read_File([DataSourceRequest] DataSourceRequest request)
    {
        var csv = new List<CSVModel>();
        
        var lines = System.IO.File.ReadAllLines(@"C:\Users\papachristouj\source\repos\MvcMovie\MvcMovie\Files\IMDB-Movie-Data.csv");
        foreach (var line in lines)
        {
            csv.Add(new CSVModel
            {
                Rank = line.Split(',')[0],
                Title = line.Split(',')[1],
                Genre = line.Split(',')[2],
                Description = line.Split(',')[3],
                Directors = line.Split(',')[4],
                Actors = line.Split(',')[5],
                Year = line.Split(',')[6],
                Runtime = line.Split(',')[7],
                Rating = line.Split(',')[8],
                Votes = line.Split(',')[9],
                Revenue = line.Split(',')[10],
                Metascore = line.Split(',')[11]
            });
        }
        string json = JsonConvert.SerializeObject(csv);

        return Json(csv.ToDataSourceResult(request));
    }

    [Route("upload-file", Name = "Upload_File"), HttpPost]
    public ActionResult Upload_File(FileModel model)
    {
        
        string folder = "C:\\Users\\papachristouj\\source\\repos\\MvcMovie\\MvcMovie\\Uploaded Files\\";
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        // save the files to the folder
       
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
        string filePath = Path.Combine(folder, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            model.File.CopyTo(stream);
        }
        // convert file to jpg
        using (var image = Image.FromStream(model.File.OpenReadStream()))
        using (var newImage = new Bitmap(image))
        using (var graphics = Graphics.FromImage(newImage))
        using (var stream = new FileStream(Path.Combine(folder, "image.jpg"), FileMode.Create))
        {
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            newImage.Save(stream, ImageFormat.Jpeg);
        }

        return Ok();
    }



    [Route("get-categories", Name = "Get_Categories_Json")]
    public IActionResult Get_Categories_Json([DataSourceRequest] DataSourceRequest request)
    {
        var model = _movieTypeService.GetCollection();

        return Json(model.Items.ToDataSourceResult(request));
    }



    [Route("modal", Name = "Modal")]
    public IActionResult Modal()
    {

        return PartialView("_Dialog");
    }



}