using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using MovieRental.Controllers;
using MovieRental.Models;
using NUnit.Framework;

namespace MovieRental.Tests;

[TestFixture]
public class MovieControllerTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    private MovieController _movieController;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json") // Adjust this file path as needed.
            .Build();

        _movieController = new MovieController(configuration);
    }

    [Test]
    public async Task CheckMovieAmount()
    {
        var result =  _movieController.FilterMovies(
            title: null, 
            category: "All", 
            minRating: null, 
            maxRating: null, 
            startYear: null, 
            endYear: null);

        var resultValue = (OkObjectResult)result;
        var movies = resultValue.Value as List<Movie>;
        movies.Should()
            .NotBeNull();
        movies.Count.Should()
            .BeGreaterThan(30);
    }
    
    [Test]
    public async Task CheckMovieRanking()
    {
        var minRating = 6;
        var maxRating = 7;
        
        var result =  _movieController.FilterMovies(
            title: null, 
            category: "All", 
            minRating: minRating, 
            maxRating: maxRating, 
            startYear: null, 
            endYear: null);

        var resultValue = (OkObjectResult)result;
        var movies = resultValue.Value as List<Movie>;
        movies.Should()
            .NotBeNull();
        movies.Should()
            .OnlyContain(movie => movie.Rating >= minRating 
                && movie.Rating <= maxRating);
    }
    
    [TestCase("Horror")]
    [TestCase("Thriller")]
    [TestCase("Romance")]
    [TestCase("Comedy")]
    public async Task CheckMovieCategories(string categoryFilter)
    {
        var result =  _movieController.FilterMovies(
            title: null, 
            category: categoryFilter, 
            minRating: null, 
            maxRating: null, 
            startYear: null, 
            endYear: null);

        var resultValue = (OkObjectResult)result;
        var movies = resultValue.Value as List<Movie>;
        movies.Should()
            .NotBeNull();
        movies.Should()
            .OnlyContain(movie => movie.MovieCategory == categoryFilter);
    }
}