using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("/api/cities")]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;
    private const int maxCitiesPageSize = 20;
    
    
    public CitiesController(ICityInfoRepository citiesInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = citiesInfoRepository ?? throw new ArgumentNullException(nameof(citiesInfoRepository));
        _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> 
        GetCities([FromQuery]string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
    {
        if (pageSize > maxCitiesPageSize)
        {
            pageSize = maxCitiesPageSize;
        }
        
        var (cityEntities, paginationMetadata) = await _cityInfoRepository.GetCitiesAsync(
            name, searchQuery, pageNumber, pageSize);
        
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        
        return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        // TODO Look into the difference between ActionResult and IActionResult. 
    {
        var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
        
        if (city == null)
        {
            return NotFound();
        }
        if (includePointsOfInterest)
        {
            return Ok(_mapper.Map<CityDto>(city));
        }

        return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
    }
}