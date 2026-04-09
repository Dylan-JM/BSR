namespace BSR.Models;

public class GeoNamesResponse
{
    public List<GeoNamesCity> Geonames { get; set; }
}

public class GeoNamesCity
{
    public string Name { get; set; }
    public string CountryCode { get; set; }
    public int Population { get; set; }
    public string Fcode { get; set; }
}
