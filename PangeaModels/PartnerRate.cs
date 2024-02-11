namespace Pangea.Models;

public class PartnerData 
{
    public List<PartnerRate> PartnerRates { get; set; } = null!;
}

public class PartnerRate
{
    // worth making the string fields into enums?

    public string Currency { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string DeliveryMethod { get; set; } = null!;

    public double Rate { get; set; } = 0.0;

    public DateTime? AcquiredDate { get; set; } = null!;
}
