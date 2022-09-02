using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContext;

public class CityInfoContext : Microsoft.EntityFrameworkCore.DbContext
{

    public DbSet<City> Cities { get; set; } = null!;
    
    public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

    public CityInfoContext(DbContextOptions<CityInfoContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
            new City("New York City")
            {
                Id = 1,
                Description = "The one with that big park.",
            },
            new City("Antwerp ")
            {
                Id = 2,
                Description = "The one with the cathedral that was never really completed.",
            },
            new City("Paris")
            {
                Id = 3,
                Description = "The one with that big tower.",
            });

        modelBuilder.Entity<PointOfInterest>().HasData(
            new PointOfInterest("Central Park")
            {
                Id = 1,
                Description = " The big ol park",
                CityId = 1
            },
            new PointOfInterest("Grand Central Station")
            {
                Id = 2,
                Description = "Train Station",
                CityId = 1
                
            },
            new PointOfInterest("Building 1")
            {
                Id = 3,
                Description = " The big building 1",
                CityId = 2
                
            },
            new PointOfInterest("the bridge")
            {
                Id = 4,
                Description = " The big bridge",
                CityId = 2
                
            },
            new PointOfInterest("sur la table")
            {
                Id = 5,
                Description = "Good food",
                CityId = 3
                
            },
            new PointOfInterest("Eiffel Tower")
            {
                Id = 6,
                Description = " The big thing 6",
                CityId = 3
                
            });
    }
}