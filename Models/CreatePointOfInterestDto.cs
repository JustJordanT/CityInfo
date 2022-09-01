using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models;

public class CreatePointOfInterestDto
{
    [Required(ErrorMessage = "You must provide a Name")]
    [MaxLength(50, ErrorMessage = "Name is too long and reached maximum length of 50 characters")]
    public string Name { get; set; } = String.Empty;
    
    [MaxLength(100, ErrorMessage = "Description is too long and reached maximum length of 100 characters")]
    public string Description { get; set; }
}