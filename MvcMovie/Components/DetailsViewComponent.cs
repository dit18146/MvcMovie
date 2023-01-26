using Microsoft.AspNetCore.Mvc;
using MvcMovie.Services;

namespace MvcMovie.Components
{
    [ViewComponent(Name = "Details")]
    public class DetailsViewComponent : ViewComponent
    {
        private readonly IMovieService _movieService;

        private readonly IMovieTypeService _movieTypeService;

        public DetailsViewComponent(IMovieService movieService, IMovieTypeService movieTypeService)
        {
            _movieService= movieService;

             _movieTypeService = movieTypeService;
        }

        public IViewComponentResult Invoke(int? id)
        {
            var model = _movieService.GetById(id);

            return View(model);
        }
    }
}
