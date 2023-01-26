using Microsoft.AspNetCore.Mvc;
using MvcMovie.Services;

namespace MvcMovie.Components
{

    [ViewComponent(Name = "MovieTypeDetails")]

    public class MovieTypeDetailsViewComponent : ViewComponent
    {
         private readonly IMovieTypeService _movieTypeService;

        public MovieTypeDetailsViewComponent(IMovieTypeService movieTypeService)
        {
            _movieTypeService = movieTypeService;
        }

        public IViewComponentResult Invoke(int id)
        {
            var model = _movieTypeService.GetById(id);

            return View(model);
        }
    }
}
