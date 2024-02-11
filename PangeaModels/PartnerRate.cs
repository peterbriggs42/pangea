namespace Pangea.Models;

public class PartnerData 
{
    public List<PartnerRate> PartnerRates { get; set; } = null!;
}

public class PartnerRate
{
    // TODO consider making the string fields into enums

    public long Id { get; set; }

    public string Currency { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string DeliveryMethod { get; set; } = null!;

    public decimal Rate { get; set; } = 0.0m;

    public DateTime? AcquiredDate { get; set; } = null!;
}
