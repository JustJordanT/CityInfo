using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Profiles;

public class PointOfInterestProfile : Profile
{
    public PointOfInterestProfile()
    {
        CreateMap<Entities.PointOfInterest, PointOfInterestDto>();
        CreateMap<CreatePointOfInterestDto, Entities.PointOfInterest>();
        CreateMap<UpdatePointOfInterestDto, Entities.PointOfInterest>();
        CreateMap<Entities.PointOfInterest, UpdatePointOfInterestDto>();
    }
}