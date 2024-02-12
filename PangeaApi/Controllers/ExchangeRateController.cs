using Microsoft.AspNetCore.Mvc;
using Pangea.Models;

namespace Pangea.Api.Controllers;

public class CountryCurrencyMapping 
{
    public string Country { get; set; } = null!;
    public string CountryCode { get; set; } = null!;
    
    public string CurrencyCode { get; set; } = null!;

    public decimal ExchangeRateAdjustment { get; set; } = 0.0m;
}

[ApiController]
[Route("api/exchange-rates")]
public class ExchangeRateController : ControllerBase
{
    // TODO this should probably go in db...in Prod system it'd be a lookup table rather than in-memory list
    private static readonly CountryCurrencyMapping[] Countries =
    [
        new CountryCurrencyMapping 
        {
            Country = "Mexico",
            CountryCode = "MEX",
            CurrencyCode = "MXN",
            ExchangeRateAdjustment = 0.024m
        },
        new CountryCurrencyMapping 
        {
            Country = "India",
            CountryCode = "IND",
            CurrencyCode = "INR",
            ExchangeRateAdjustment = 3.213m
        },
        new CountryCurrencyMapping 
        {
            Country = "Phillippines",
            CountryCode = "PHL",
            CurrencyCode = "PHP",
            ExchangeRateAdjustment = 2.437m
        },
        new CountryCurrencyMapping 
        {
            Country = "Guatemala",
            CountryCode = "GTM",
            CurrencyCode = "GTQ",
            ExchangeRateAdjustment = 0.056m
        },
    ];

    /* TODO 
    1. test the 400 behavior for country code
      - DONE
    2. ingest the json file inside of the API startup logic
      - DONE
    3. fetch the exchange rates matching the country code using EF
      - DONE
    4. stick that logic inside of an injectable service
      - WON'T DO
    5. write unit tests against the service
      - DONE
    6. put the ingest logic also in the service and write tests against it?
      - NO TIME
    */ 

    // TODO if I had more time I'd implement logging. Probably more useful for the 
    //      ingest than the API I suppose...
    // private readonly ILogger<ExchangeRateController> _logger;
    private readonly PangeaContext _context;


    public ExchangeRateController(PangeaContext context)
    {
        _context = context;
    }

    // TODO what is Name supposed to be here?
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExchangeRateDto>>> Get([FromQuery] string country)
    {
        // TODO consider making `country` an enum so that swagger can provide client with valid options

        if (!validateCountry(country))
        {
            return BadRequest($"Country code not valid: {country}");
        };

        var countryCurrencyLookup = Countries.First(x => x.CountryCode == country);
        var exchangeRatesForCountry = _context.PartnerRates
            .Where(pr => pr.Currency == countryCurrencyLookup.CurrencyCode)
            .Select(pr => PartnerRateToExchangeRate(pr, countryCurrencyLookup))
            .ToList();

        return exchangeRatesForCountry;
    }

    private static ExchangeRateDto PartnerRateToExchangeRate(PartnerRate partnerRate, CountryCurrencyMapping countryCurrency)
    {
        return new ExchangeRateDto() 
        {
            CurrencyCode = countryCurrency.CurrencyCode,
            CountryCode = countryCurrency.CountryCode,
            PangeaRate = decimal.Round(partnerRate.Rate + countryCurrency.ExchangeRateAdjustment, 2, MidpointRounding.ToPositiveInfinity),
            PaymentMethod = partnerRate.PaymentMethod,
            DeliveryMethod = partnerRate.DeliveryMethod
        };
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
