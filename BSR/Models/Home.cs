namespace BSR.Models;

public class Home
{
    public int Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Area { get; set; }
    public string ImageURL { get; set; } = string.Empty;
}
