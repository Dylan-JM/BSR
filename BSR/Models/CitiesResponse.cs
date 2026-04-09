namespace BSR.Models;

public class CitiesResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int State_Id { get; set; }
}

public class Data
{
    public List<CityData> Cities { get; set; }
}

public class CityData
{
    public string Name { get; set; }
}
