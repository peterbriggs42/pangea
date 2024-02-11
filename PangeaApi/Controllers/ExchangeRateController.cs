using Microsoft.AspNetCore.Mvc;
using Pangea.Models;

namespace Pangea.Api.Controllers;

public class CountryCurrencyMapping 
{
    public string Country { get; set; } = null!;
    public string CountryCode { get; set; } = null!;
    
    public string CurrencyCode { get; set; } = null!;
}

[ApiController]
[Route("api/exchange-rates")]
public class ExchangeRateController : ControllerBase
{
    private static readonly CountryCurrencyMapping[] Countries = new[]
    {
        new CountryCurrencyMapping 
        {
            Country = "Mexico",
            CountryCode = "MEX",
            CurrencyCode = "MXN"
        },
        new CountryCurrencyMapping 
        {
            Country = "India",
            CountryCode = "IND",
            CurrencyCode = "INR"
        },
        new CountryCurrencyMapping 
        {
            Country = "Phillippines",
            CountryCode = "PHL",
            CurrencyCode = "PHP"
        },
        new CountryCurrencyMapping 
        {
            Country = "Guatemala",
            CountryCode = "GTM",
            CurrencyCode = "GTQ"
        },
    };

    /* TODO 
    1. test the 400 behavior for country code
    2. ingest the json file inside of the API startup logic
    3. fetch the exchange rates matching the country code using EF
    4. stick that logic inside of an injectable service
    5. write unit tests against the service
    6. put the ingest logic also in the service and write tests against it?
    
    */ 

    private readonly ILogger<ExchangeRateController> _logger;

    public ExchangeRateController(ILogger<ExchangeRateController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetBlahBlah")]
    public async Task<ActionResult<IEnumerable<ExchangeRateDto>>> Get([FromQuery] string country)
    {
        if (!validateCountry(country))
        {
            return NotFound();
        };

        // instantiate a context and await it with a FindAsync

        return new List<ExchangeRateDto>();
    }

    private bool validateCountry(string countryCode)
    {
        var countryCodes = Countries.Select(c => c.CountryCode).ToList();
        if (!countryCodes.Contains(countryCode)) 
        {
            return false;
        }
        return true;
    }
}
