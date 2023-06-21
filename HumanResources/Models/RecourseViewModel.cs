using System.ComponentModel.DataAnnotations;


namespace HumanResources.Models;

public class RecourseViewModel
{
    public string? Name { get; set; }
    public string? Number { get; set; }
    public string? Email { get; set; }




    public Guid AdvertId { get; set; }
}
