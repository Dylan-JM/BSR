namespace BSR.Models;

public class GeoNamesResponse
{
    public List<GeoNamesItem> Geonames { get; set; }
}

public class GeoNamesItem
{
    public string Name { get; set; }
    public string CountryCode { get; set; }
    public string AdminCode2 { get; set; }
    public string Fcode { get; set; }
    public int Population { get; set; }
}
