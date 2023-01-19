using CsvHelper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Extensions;
using MvcMovie.Models;
using MvcMovie.Services;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MvcMovie.Controllers
{
    [Route("kendo")]
    public class KendoController : Controller
    {
        
        private readonly IFileservice _fileService;

        private readonly IWebHostEnvironment _env;

        private static string fileName;

        private static long newsletterId;

         public KendoController(IFileservice fileService,  IWebHostEnvironment env)
         {
            _fileService = fileService;

            _env = env;
         }

        [Route("Index", Name = "Index")]
        public IActionResult Kendo_Index()
        {
            var model = _fileService.GetCollection();

            return View(model);
        }

        [Route("upload-file", Name = "Upload_File")]
        [HttpPost]
        public ActionResult Upload_File(FileModel model)
        {
      
            string path = FindPath.MapPath("UploadedFiles");
      
            // save the files to the folder

            fileName = $"{DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss")}_{Guid.NewGuid()}{Path.GetExtension(model.File.FileName)}";

            newsletterId = Int32.Parse(Regex.Match(model.File.FileName, "\\d+").ToString());

            var filePath = Path.Combine(path, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }
           
            var xhr = Read_File();

            if (xhr == 200)

                 return Ok();

            else if (xhr == 400)

                return StatusCode(400);

            else

                return StatusCode(500);

           
           
        }

        [Route("read-file", Name = "Read_File")]
        public int Read_File()
        {
      
            string path = FindPath.MapPath("UploadedFiles");

            path = path + "//" + fileName;

           

            using (var reader = new StreamReader(path))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CSVModel>();

                csv.Read();
                csv.ReadHeader();
                _fileService.DeleteAll();

                try
                {
                      while (csv.Read())
                      {
                        CSVModel record = new CSVModel();

                        record.NewsletterId = newsletterId;
                        record.CustomerId = csv.GetField<long>("CustomerId");
                        record.Username = csv.GetField<string>("Login");
                        record.X1 = csv.GetField<string>("X1");
                        _fileService.Add(record);
                        
                      }
                }
                catch(Exception ex) {
                    CSVModel errorModel = new CSVModel();
                    _fileService.DeleteAll();

                    if(ex.Source == "CsvHelper")
                        return 400;

                    else if(ex.Source == "System.Data.SQLite")
                        return 500;
                    
                }
                return 200;
               
            }
        }
        
     

         public ActionResult Read_Database([DataSourceRequest] DataSourceRequest request)
         {
            var records = _fileService.GetCollection();
           
             try
             {
                    return Json(records.Items.ToDataSourceResult(request));

             }
                catch (Exception e)
                {
                
                   int[] arr = new int[0];

                    return Json(arr.ToDataSourceResult(request));
                }
         }

    }
}
