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

    public async Task<List<string>> GetUkCities()
    {
        var http = _httpClientFactory.CreateClient();

        var url =
            $"http://api.geonames.org/searchJSON?country=GB&featureClass=P&maxRows=1000&orderby=population&username={_geoNamesUser}";

        var json = await http.GetStringAsync(url);

        var result = JsonConvert.DeserializeObject<GeoNamesResponse>(json);

        return result.Geonames.Select(x => x.Name).Distinct().OrderBy(x => x).ToList();
    }
}
