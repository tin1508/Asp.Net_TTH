namespace StationeryStore.Mvc.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public List<Stationery> Stationeries { get; set; } = new List<Stationery>();
}