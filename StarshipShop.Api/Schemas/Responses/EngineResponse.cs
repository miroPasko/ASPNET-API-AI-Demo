namespace StarshipShop.Api.Schemas.Responses;

public class EngineResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public double MaximumSpeed { get; set; }
    public double FuelUsage { get; set; }
    public string Manufacturer { get; set; } = null!;
    public decimal Price { get; set; }
}
