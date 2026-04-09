using BSR.Models;
using Newtonsoft.Json;

namespace BSR.Services;

public class AddressService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiKey;

    public AddressService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _apiKey = config["CSC:ApiKey"];
    }

    public List<string> GetUkCities()
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-CSCAPI-KEY", _apiKey);

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "https://api.countrystatecity.in/v1/countries/GB/cities"
        );

        try
        {
            var response = httpClient.Send(request);
            if (!response.IsSuccessStatusCode)
                return new List<string>();

            var json = response.Content.ReadAsStringAsync().Result;
            var cities = JsonConvert.DeserializeObject<List<CitiesResponse>>(json);

            return cities.Select(c => c.Name).ToList();
        }
        catch
        {
            return new List<string>();
        }
    }
}
