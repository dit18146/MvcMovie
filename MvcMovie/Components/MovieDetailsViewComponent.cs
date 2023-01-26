using Microsoft.AspNetCore.Mvc;
using MvcMovie.Services;

namespace MvcMovie.Components;

[ViewComponent(Name = "MovieDetails")]
public class MovieDetailsViewComponent : ViewComponent
{
    private readonly IMovieService _movieService;

    /// <inheritdoc />
    public MovieDetailsViewComponent(IMovieService movieService) => _movieService = movieService;

    public IViewComponentResult Invoke(int id)
    {
        var model = _movieService.GetById(id);

        return View(model);
    }
}