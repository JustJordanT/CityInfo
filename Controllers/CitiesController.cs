using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("/api/cities")]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    public CitiesController(ICityInfoRepository citiesInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = citiesInfoRepository ?? throw new ArgumentNullException(nameof(citiesInfoRepository));
        _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
    {
        var cityEntities = await _cityInfoRepository.GetCitiesAsync();
        var results = new List<CityWithoutPointsOfInterestDto>(); 
        
        // var results = cityEntities.Select(cityEntity => new CityWithoutPointsOfInterestDto { Id = cityEntity.Id, Name = cityEntity.Name, Description = cityEntity.Description, }).ToList();
        foreach (var cityEntity in cityEntities)
        {
            results.Add(new CityWithoutPointsOfInterestDto
            {
                Id = cityEntity.Id,
                Name = cityEntity.Name,
                Description = cityEntity.Description,
            });
        }

        return Ok(results);
    }

    // [HttpGet("{id}")]
    // public ActionResult<CityDto> GetCity(int id)
    // {
    //     
    //     var cityToReturn = _cityInfoRepository.GetCityAsync()
    //         FirstOrDefault(c => c.Id == id);
    //     if (cityToReturn == null)
    //     {
    //         return NotFound();
    //     }
    //     
    //     return Ok(cityToReturn);
    // }
}