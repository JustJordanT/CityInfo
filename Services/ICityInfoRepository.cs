using System.Drawing;
using CityInfo.API.Entities;

namespace CityInfo.API.Services;

public interface ICityInfoRepository
{
   
   Task<IEnumerable<City>> GetCitiesAsync();
   Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
   Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
   
   Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
   Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
   Task<bool> CityExistsAsync(int cityId);
   Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
   Task<bool> SaveChangesAsync();
   void DeletePointOfInterest(PointOfInterest pointOfInterest);
   
   
}