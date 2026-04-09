namespace Zucchini.Domain.Entities;

public class GeoLocation
{
    public string Name { get; set; } = string.Empty;
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}
