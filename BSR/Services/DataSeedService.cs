using Bogus;
using BSR.Models;

namespace BSR.Services;

public class DataSeedService
{
    private readonly HomeContext _homeContext;

    public DataSeedService(HomeContext homeContext)
    {
        _homeContext = homeContext;
    }

    private static readonly Dictionary<string, List<string>> CountiesToCities = new()
    {
        {
            "Bedfordshire",
            new List<string> { "Bedford", "Luton", "Dunstable" }
        },
        {
            "Berkshire",
            new List<string> { "Reading", "Slough", "Maidenhead", "Windsor" }
        },
        {
            "Bristol",
            new List<string> { "Bristol", "Southmead", "Filton" }
        },
        {
            "Buckinghamshire",
            new List<string> { "Aylesbury", "High Wycombe", "Slough", "Beaconsfield" }
        },
        {
            "Cambridgeshire",
            new List<string> { "Cambridge", "Peterborough", "Ely" }
        },
        {
            "Cheshire",
            new List<string> { "Chester", "Warrington", "Stockport" }
        },
        {
            "City of London",
            new List<string> { "London" }
        },
        {
            "Cornwall",
            new List<string> { "Truro", "Penzance", "Bodmin" }
        },
        {
            "County Durham",
            new List<string> { "Durham", "Sunderland", "Darlington" }
        },
        {
            "Cumbria",
            new List<string> { "Carlisle", "Kendal", "Penrith" }
        },
        {
            "Derbyshire",
            new List<string> { "Derby", "Chesterfield", "Ilkeston" }
        },
        {
            "Devon",
            new List<string> { "Exeter", "Plymouth", "Barnstaple", "Torquay" }
        },
        {
            "Dorset",
            new List<string> { "Dorchester", "Poole", "Weymouth", "Bournemouth" }
        },
        {
            "East Riding of Yorkshire",
            new List<string> { "Kingston upon Hull", "Beverley", "Goole" }
        },
        {
            "East Sussex",
            new List<string> { "Brighton", "Eastbourne", "Hastings" }
        },
        {
            "Essex",
            new List<string> { "Chelmsford", "Southend-on-Sea", "Colchester", "Basildon" }
        },
        {
            "Gloucestershire",
            new List<string> { "Gloucester", "Cheltenham", "Forest of Dean" }
        },
        {
            "Greater London",
            new List<string> { "London", "Westminster", "Camden", "Kensington" }
        },
        {
            "Greater Manchester",
            new List<string> { "Manchester", "Bolton", "Oldham", "Stockport" }
        },
        {
            "Hampshire",
            new List<string> { "Winchester", "Southampton", "Portsmouth", "Basingstoke" }
        },
        {
            "Herefordshire",
            new List<string> { "Hereford", "Ross-on-Wye", "Leominster" }
        },
        {
            "Hertfordshire",
            new List<string> { "Hertford", "Watford", "St Albans" }
        },
        {
            "Isle of Wight",
            new List<string> { "Newport", "Sandown", "Shanklin" }
        },
        {
            "Kent",
            new List<string> { "Maidstone", "Canterbury", "Dover", "Gillingham" }
        },
        {
            "Lancashire",
            new List<string> { "Preston", "Liverpool", "Manchester", "Burnley" }
        },
        {
            "Leicestershire",
            new List<string> { "Leicester", "Loughborough", "Hinckley" }
        },
        {
            "Lincolnshire",
            new List<string> { "Lincoln", "Boston", "Grantham" }
        },
        {
            "Merseyside",
            new List<string> { "Liverpool", "Birkenhead", "Wallasey" }
        },
        {
            "Norfolk",
            new List<string> { "Norwich", "Great Yarmouth", "King's Lynn" }
        },
        {
            "North Yorkshire",
            new List<string> { "York", "Harrogate", "Scarborough", "Leeds" }
        },
        {
            "Northamptonshire",
            new List<string> { "Northampton", "Kettering", "Wellingborough" }
        },
        {
            "Northumberland",
            new List<string> { "Newcastle upon Tyne", "Sunderland", "Gateshead" }
        },
        {
            "Nottinghamshire",
            new List<string> { "Nottingham", "Mansfield", "Newark" }
        },
        {
            "Oxfordshire",
            new List<string> { "Oxford", "Banbury", "Witney" }
        },
        {
            "Rutland",
            new List<string> { "Oakham", "Uppingham" }
        },
        {
            "Shropshire",
            new List<string> { "Shrewsbury", "Telford", "Ludlow" }
        },
        {
            "Somerset",
            new List<string> { "Bath", "Bristol", "Taunton", "Wells" }
        },
        {
            "South Yorkshire",
            new List<string> { "Sheffield", "Rotherham", "Doncaster" }
        },
        {
            "Staffordshire",
            new List<string> { "Stafford", "Stoke-on-Trent", "Lichfield" }
        },
        {
            "Suffolk",
            new List<string> { "Ipswich", "Lowestoft", "Sudbury" }
        },
        {
            "Surrey",
            new List<string> { "Guildford", "Woking", "Croydon", "Sutton" }
        },
        {
            "Tyne and Wear",
            new List<string> { "Newcastle upon Tyne", "Sunderland", "Gateshead" }
        },
        {
            "Warwickshire",
            new List<string> { "Warwick", "Coventry", "Rugby" }
        },
        {
            "West Midlands",
            new List<string> { "Birmingham", "Wolverhampton", "Coventry" }
        },
        {
            "West Sussex",
            new List<string> { "Chichester", "Worthing", "Crawley" }
        },
        {
            "West Yorkshire",
            new List<string> { "Leeds", "Bradford", "Wakefield", "Halifax" }
        },
        {
            "Wiltshire",
            new List<string> { "Salisbury", "Swindon", "Trowbridge" }
        },
        {
            "Worcestershire",
            new List<string> { "Worcester", "Redditch", "Bromsgrove" }
        },
    };

    public void SeedHomes()
    {
        var homes = new List<Home>();

        var imageUrls = new List<string>
        {
            "https://images.unsplash.com/photo-1568605114967-8130f3a36994",
            "https://images.unsplash.com/photo-1570129477492-45c003edd2be",
            "https://plus.unsplash.com/premium_photo-1661964475795-f0cb85767a88",
            "https://images.unsplash.com/photo-1580587771525-78b9dba3b914",
            "https://images.unsplash.com/photo-1613977257363-707ba9348227",
            "https://images.pexels.com/photos/106399/pexels-photo-106399.jpeg",
            "https://images.pexels.com/photos/9951999/pexels-photo-9951999.jpeg",
            "https://images.pexels.com/photos/5178060/pexels-photo-5178060.jpeg",
            "https://images.pexels.com/photos/8583869/pexels-photo-8583869.jpeg",
            "https://images.pexels.com/photos/7710011/pexels-photo-7710011.jpeg",
            "https://images.pexels.com/photos/5524205/pexels-photo-5524205.jpeg",
        };

        if (!_homeContext.Homes.Any())
        {
            var faker = new Faker("en");

            // Create homes for each county with varied filter values
            foreach (var county in CountiesToCities.Keys)
            {
                var cities = CountiesToCities[county];

                // Create x homes per county with different filter combinations (set to one to save azure credits)
                for (int j = 0; j < 1; j++)
                {
                    var home = new Home
                    {
                        Price = (200000 + (j * 80000)) + faker.Random.Int(-20000, 20000),
                        StreetAddress = $"{faker.Random.Int(1, 500)} {faker.Address.StreetName()}",
                        County = county,
                        City = cities[faker.Random.Int(0, cities.Count - 1)],
                        Area = 150 + (j * 50) + faker.Random.Int(-30, 30),
                        Bedrooms = (j % 5) + 1,
                        Bathrooms = ((j + 1) % 5) + 1,
                        GarageSpots = (j % 4),
                        ImageUrl = imageUrls[j % imageUrls.Count],
                    };

                    homes.Add(home);
                }
            }

            _homeContext.Homes.AddRange(homes);
        }

        _homeContext.SaveChanges();
    }
}
