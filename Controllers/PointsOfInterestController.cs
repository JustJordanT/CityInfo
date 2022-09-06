using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
        }
        
        
        [HttpGet] // GET method for getting points of interestDto
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest");
                return NotFound();
            }
            var pointsofinterestForCity =
                await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsofinterestForCity));
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointsOfInterestById(
            int cityId, int pointofinterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest");
                return NotFound();
            }
            
            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointofinterestId);
            switch (pointOfInterest)
            {
                case null:
                    return NotFound();
            }
            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));

        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId,
            CreatePointOfInterestDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);
            
            await _cityInfoRepository.SaveChangesAsync();

            var createdPointOfInterestToReturn = _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId,
                pointOfInterestId = createdPointOfInterestToReturn.Id
            }, createdPointOfInterestToReturn);

        }

        [HttpPut("{pointofinterestid}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
            UpdatePointOfInterestDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) // This is a null check from our Repository Service.
            {
                return NotFound();
            }

            var pointOfInterestEntity =
                await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();

            }

            _mapper.Map(pointOfInterest, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        //
            [HttpPatch("{pointofinterestid}")]
            public async Task<ActionResult> PartialUpdatePointOfInterest(int cityId, int pointOfInterestId,
                JsonPatchDocument<UpdatePointOfInterestDto> patchDocument)
            {

                if (!await _cityInfoRepository.CityExistsAsync(cityId))
                {
                    return NotFound();
                }


                var pointOfInterestEntity =
                    await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
                if (pointOfInterestEntity == null)
                {
                    return NotFound();
                }

                var pointOfInterestToPatch = _mapper.Map<UpdatePointOfInterestDto>(pointOfInterestEntity);
                
                patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!TryValidateModel(pointOfInterestToPatch))
                {
                    return BadRequest(ModelState);
                }

                _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
                
                await _cityInfoRepository.SaveChangesAsync();
                
                return NoContent();

            }
            
            [HttpDelete("{pointOfInterestId}")]
            public async Task<IActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
            //When using id instead of cityId i got a 404 error (╯°□°）╯︵ ┻━┻
            {
                if (!await _cityInfoRepository.CityExistsAsync(cityId))
                {
                    return NotFound();
                }


                var pointOfInterestEntity =
                    await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
                if (pointOfInterestEntity == null)
                {
                    return NotFound();
                }

                _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
                await _cityInfoRepository.SaveChangesAsync();
                
                _mailService.SendMail("point Of Interest deleted", $"Point Of Interest {pointOfInterestEntity.Name}" +
                                                                   $"with id {pointOfInterestEntity.Id} was deleted successfully");
                return NoContent();
            }
    }
}