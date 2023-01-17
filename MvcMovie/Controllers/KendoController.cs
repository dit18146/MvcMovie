using CsvHelper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Extensions;
using MvcMovie.Models;
using MvcMovie.Services;
using System.Globalization;

namespace MvcMovie.Controllers
{
    [Route("kendo")]
    public class KendoController : Controller
    {
        
        private readonly IMovieService _movieService;

        private readonly IMovieTypeService _movieTypeService;

        private readonly IWebHostEnvironment _env;

        private static string fileName;

         public KendoController(IMovieService movieService, IMovieTypeService movieTypeService, IWebHostEnvironment env)
         {
            _movieService = movieService;

            _movieTypeService = movieTypeService;

            _env = env;
         }

        [Route("Index", Name = "Index")]
        public IActionResult Kendo_Index()
        {
            var model = _movieService.GetCollection();

            return View(model);
        }

        [Route("upload-file", Name = "Upload_File")]
        [HttpPost]
        public ActionResult Upload_File(FileModel model)
        {
      
            string path = FindPath.MapPath("UploadedFiles");
      
            // save the files to the folder

            fileName = $"{DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss")}_{Guid.NewGuid()}{Path.GetExtension(model.File.FileName)}"; 
            var filePath = Path.Combine(path, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }

            return Ok();
        }

        [Route("read-file", Name = "Read_File")]
        public ActionResult Read_File([DataSourceRequest] DataSourceRequest request)
        {
      
            string path = FindPath.MapPath("UploadedFiles");
            path = path + "//" + fileName;

            if(path.IsNullOrEmptyOrWhiteSpace())
                return NotFound();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CSVModel>();

                try
                {
                    return Json(records.ToArray().ToDataSourceResult(request));
                }
                catch (Exception e)
                {
                
                   int[] arr = new int[0];
                    return Json(arr.ToDataSourceResult(request));
                }
           
            }
        }

        [Route("json-kendo", Name = "Kendo_Json_Index")]
        public IActionResult Kendo_Json([DataSourceRequest] DataSourceRequest request)
        {
            var model = _movieService.GetCollection();

            return Json(model.Items.ToDataSourceResult(request));
        }


    }
}
