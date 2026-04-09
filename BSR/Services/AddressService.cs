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

    private static readonly Dictionary<string, string> UkCounties = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Bedfordshire", "Bedfordshire" },
        { "Berkshire", "Berkshire" },
        { "Bristol", "Bristol" },
        { "Buckinghamshire", "Buckinghamshire" },
        { "Cambridgeshire", "Cambridgeshire" },
        { "Cheshire", "Cheshire" },
        { "City of London", "City of London" },
        { "Cornwall", "Cornwall" },
        { "County Durham", "County Durham" },
        { "Cumbria", "Cumbria" },
        { "Derbyshire", "Derbyshire" },
        { "Devon", "Devon" },
        { "Dorset", "Dorset" },
        { "East Riding of Yorkshire", "East Riding of Yorkshire" },
        { "East Sussex", "East Sussex" },
        { "Essex", "Essex" },
        { "Gloucestershire", "Gloucestershire" },
        { "Greater London", "Greater London" },
        { "Greater Manchester", "Greater Manchester" },
        { "Hampshire", "Hampshire" },
        { "Herefordshire", "Herefordshire" },
        { "Hertfordshire", "Hertfordshire" },
        { "Isle of Wight", "Isle of Wight" },
        { "Kent", "Kent" },
        { "Lancashire", "Lancashire" },
        { "Leicestershire", "Leicestershire" },
        { "Lincolnshire", "Lincolnshire" },
        { "Merseyside", "Merseyside" },
        { "Norfolk", "Norfolk" },
        { "North Yorkshire", "North Yorkshire" },
        { "Northamptonshire", "Northamptonshire" },
        { "Northumberland", "Northumberland" },
        { "Nottinghamshire", "Nottinghamshire" },
        { "Oxfordshire", "Oxfordshire" },
        { "Rutland", "Rutland" },
        { "Shropshire", "Shropshire" },
        { "Somerset", "Somerset" },
        { "South Yorkshire", "South Yorkshire" },
        { "Staffordshire", "Staffordshire" },
        { "Suffolk", "Suffolk" },
        { "Surrey", "Surrey" },
        { "Tyne and Wear", "Tyne and Wear" },
        { "Warwickshire", "Warwickshire" },
        { "West Midlands", "West Midlands" },
        { "West Sussex", "West Sussex" },
        { "West Yorkshire", "West Yorkshire" },
        { "Wiltshire", "Wiltshire" },
        { "Worcestershire", "Worcestershire" }
    };

    public Task<Dictionary<string, string>> GetUkCounties()
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

        return result?
            .Geonames
            .Select(x => x.Name)
            .Distinct()
            .OrderBy(x => x)
            .ToList() ?? new List<string>();
    }
}
