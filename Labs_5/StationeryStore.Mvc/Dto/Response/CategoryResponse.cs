using StationeryStore.Mvc.Controllers;

namespace StationeryStore.Mvc.Dto.Response;

public class CategoryResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;
    public int StationeriesCount {get; set;}
}