
using Microsoft.AspNetCore.Mvc;
using WebApplication15.Models;

namespace WebApplication15.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private static readonly List<Movie> movies = new()
    {
        new Movie
        {
            Id = 1,
            Title = "Inception",
            Genre = "Sci-Fi",
            Year = 2010,
            Director = "Christopher Nolan"
        },
        new Movie
        {
            Id = 2,
            Title = "Interstellar",
            Genre = "Sci-Fi",
            Year = 2014,
            Director = "Christopher Nolan"
        },
        new Movie
        {
            Id = 3,
            Title = "Titanic",
            Genre = "Romance",
            Year = 1997,
            Director = "James Cameron"
        },
        new Movie
        {
            Id = 4,
            Title = "Avatar",
            Genre = "Fantasy",
            Year = 2009,
            Director = "James Cameron"
        }
    };

    [HttpGet]
    public IActionResult Index(
        string? search,
        string? genre,
        int? year,
        string? sortBy,
        string? sortOrder)
    {
        IEnumerable<Movie> result = movies;

        if (!string.IsNullOrWhiteSpace(search))
        {
            result = result.Where(m =>
                m.Title.Contains(search,
                    StringComparison.OrdinalIgnoreCase) ||
                m.Director.Contains(search,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            result = result.Where(m =>
                m.Genre.Equals(genre,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (year.HasValue)
        {
            result = result.Where(m => m.Year == year.Value);
        }

        bool descending = string.Equals(
            sortOrder, "desc",
            StringComparison.OrdinalIgnoreCase);

        result = sortBy?.ToLowerInvariant() switch
        {
            "title" => descending
                ? result.OrderByDescending(m => m.Title)
                : result.OrderBy(m => m.Title),

            "year" => descending
                ? result.OrderByDescending(m => m.Year)
                : result.OrderBy(m => m.Year),

            _ => result.OrderBy(m => m.Id)
        };

        return Ok(result.ToList());
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var movie = movies.FirstOrDefault(m => m.Id == id);

        if (movie == null)
        {
            return NotFound("Фільм не знайдено");
        }

        return Ok(movie);
    }
}