using BSR.Models;
using Newtonsoft.Json;

namespace BSR.Services;

public class AddressService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _geoNamesUser;

    public AddressService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _geoNamesUser = config["GeoNames:Username"];
    }

    private static readonly List<string> UkCounties = new()
    {
        "Bedfordshire",
        "Berkshire",
        "Bristol",
        "Buckinghamshire",
        "Cambridgeshire",
        "Cheshire",
        "City of London",
        "Cornwall",
        "County Durham",
        "Cumbria",
        "Derbyshire",
        "Devon",
        "Dorset",
        "East Riding of Yorkshire",
        "East Sussex",
        "Essex",
        "Gloucestershire",
        "Greater London",
        "Greater Manchester",
        "Hampshire",
        "Herefordshire",
        "Hertfordshire",
        "Isle of Wight",
        "Kent",
        "Lancashire",
        "Leicestershire",
        "Lincolnshire",
        "Merseyside",
        "Norfolk",
        "North Yorkshire",
        "Northamptonshire",
        "Northumberland",
        "Nottinghamshire",
        "Oxfordshire",
        "Rutland",
        "Shropshire",
        "Somerset",
        "South Yorkshire",
        "Staffordshire",
        "Suffolk",
        "Surrey",
        "Tyne and Wear",
        "Warwickshire",
        "West Midlands",
        "West Sussex",
        "West Yorkshire",
        "Wiltshire",
        "Worcestershire",
    };

    public Task<List<string>> GetUkCounties()
    {
        return Task.FromResult(UkCounties);
    }

    public async Task<List<string>> GetCitiesInCounty(string countyName)
    {
        if (string.IsNullOrWhiteSpace(countyName))
        {
            return new List<string>();
        }

        var http = _httpClientFactory.CreateClient();

        var url =
            $"http://api.geonames.org/searchJSON?country=GB&featureClass=P&adminName2={Uri.EscapeDataString(countyName)}&maxRows=1000&style=FULL&username={_geoNamesUser}";

        var json = await http.GetStringAsync(url);

        var result = JsonConvert.DeserializeObject<GeoNamesResponse>(json);

        return result?.Geonames.Select(x => x.Name).Distinct().OrderBy(x => x).ToList()
            ?? new List<string>();
    }
}
