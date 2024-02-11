namespace Pangea.Models;

public class ExchangeRateDto
{
    // TODO consider making the string fields into enums

    public string CurrencyCode { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public double PangeaRate { get; set; } = 0.0;

    public string PaymentMethod { get; set; } = null!;

    public string DeliveryMethod { get; set; } = null!;

}