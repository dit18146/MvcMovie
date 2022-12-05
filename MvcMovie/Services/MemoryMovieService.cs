using MvcMovie.Models;
using System.Drawing;
using System;
using System.Xml.Linq;

namespace MvcMovie.Services;

public class MemoryMovieService : IMovieService
{
    private readonly Movies _db = new Movies();

    private int id_increment;

    List<MovieType> MovieTypes = new()
    {
        new(id: 6, name: "Adventure"),
        new(7, "Action"),
        new(8, "Comedy"),
        new(9, "Horror"),
        new(10, "N/A"),
    };

    public string JoinCategory(int id)
    {
        var query =
        from Movie in _db.Items
        join MovieType in MovieTypes on Movie.MovieTypeId equals MovieType.Id
        where Movie.Id == id
        select new
        {
            Name = MovieType.Name
        };
        foreach (var Item in query)
        {
            return Item.Name;
        }
        return null;
    }


    public async Task<Movies?> GetCollectionAsync() => throw new NotImplementedException();

    public void Add(Movie item)
    {
        ++id_increment;

        item.Id = id_increment;

        _db.Items.Add(item);

        item.Name = JoinCategory(item.Id);
    }

    public bool CheckIfExists(int id)
    {
        foreach (var item in _db.Items )
        {
            if (item.Id == id)

                return true;
        }
        return false;
    }

    
    public void Delete(Movie item)
    {
        _db.Items.RemoveAt(item.Id - 1);
    }

    public Movie? GetById(int? id)
    {
        return _db.Items.FirstOrDefault(x => x.Id == id);
    }

    public Movies? GetCollection() => _db;

    public void Update(Movie item)
    {
        _db.Items[item.Id - 1].Title = item.Title;

        _db.Items[item.Id - 1].Description = item.Description;

        _db.Items[item.Id - 1].MovieTypeId = item.MovieTypeId;
    }

  
}