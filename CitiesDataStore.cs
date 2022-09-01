using CityInfo.API.Models;

namespace CityInfo.API;

public class CitiesDataStore
{
    public List<CityDto> Cities { get; set; }
    
    // public static CitiesDataStore Current { get; } = new CitiesDataStore();

    public CitiesDataStore()
    {
        Cities = new List<CityDto>
        {
            new CityDto()
            {
                Id = 1,
                Name = " New York City ",
                Description = " The one with that big park . ",
                PointsOfInterest = new List<PointOfInterestDto>
                {
                    new PointOfInterestDto
                    {
                        Id = 1,
                        Name = "Statue of liberty",
                        Description = "Big lady holding the light thing."
                    },
                    new PointOfInterestDto
                    {
                        Id = 2,
                        Name = "Central Park",
                        Description = "The one with the big park."
                    }
                }
            },
            new CityDto()
            {
                Id = 2,
                Name = " Antwerp ",
                Description = " The one with the cathedral that was never really",
                PointsOfInterest = new List<PointOfInterestDto>
                {
                    new PointOfInterestDto
                    {
                        Id = 3,
                        Name = "Bulding 1",
                        Description = "The one with the big bulding."
                    },
                    new PointOfInterestDto
                    {
                        Id = 4,
                        Name = "Bulding 2",
                        Description = "The one with the big bulding 2."
                    }
                }
            },
            new CityDto()
            {
                Id = 3,
                Name = " Paris ",
                Description = " The one with that big tower . ",
                PointsOfInterest = new List<PointOfInterestDto>
                {   
                    new PointOfInterestDto
                    {
                        Id = 5,
                        Name = "Thing1",
                        Description = " Thedral"
                    },
                    new PointOfInterestDto
                    {
                        Id = 6,
                        Name = "Thing2",
                        Description = " Thedral2"
                    }
                }
            }
        };
    }
}