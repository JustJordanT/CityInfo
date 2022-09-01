using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("/api/cities")]
public class CitiesController : ControllerBase
{
    private readonly CitiesDataStore _citiesDataStore;

    public CitiesController(CitiesDataStore citiesDataStore)
    {
        _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetGetCities()
    {
        return Ok(_citiesDataStore.Cities);
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var cityToReturn = _citiesDataStore.Cities.
            FirstOrDefault(c => c.Id == id);
        if (cityToReturn == null)
        {
            return NotFound();
        }
        
        return Ok(cityToReturn);
    }
}