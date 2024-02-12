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
    private static readonly CountryCurrencyMapping[] _countryCurrencyMappings =
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

    private readonly PangeaContext _context;

    public ExchangeRateController(PangeaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ExchangeRateDto>> Get([FromQuery] string country)
    {
        // TODO consider making `country` an enum so that swagger can provide client with valid options
        if (!validateCountry(country))
        {
            return BadRequest($"Country code not valid: {country}");
        };

        var countryCurrencyLookup = _countryCurrencyMappings.First(x => x.CountryCode.ToLower() == country.ToLower());
        // TODO if we were hitting a real db, we'd want to change this to ToListAsync()
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
            PangeaRate = decimal.Round(partnerRate.Rate + countryCurrency.ExchangeRateAdjustment, 2),
            PaymentMethod = partnerRate.PaymentMethod,
            DeliveryMethod = partnerRate.DeliveryMethod
        };
    }

    private bool validateCountry(string countryCode)
    {
        var countryCodes = _countryCurrencyMappings.Select(ccm => ccm.CountryCode.ToLower()).ToList();
        if (!countryCodes.Contains(countryCode.ToLower())) 
        {
            return false;
        }
        return true;
    }
}
