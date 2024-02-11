
using Newtonsoft.Json;
using Pangea.Models;

public class PartnerRateIngest : IPartnerRateIngest
{
    private PangeaContext _context;
    public PartnerRateIngest(PangeaContext context)
    {
        _context = context;
    }

    const string PARTNER_RATES_FILE_PATH = "../partner-data.json";
    public async void IngestPartnerRates()
    {
        using StreamReader reader = new(PARTNER_RATES_FILE_PATH);
        var json = reader.ReadToEnd();
        PartnerData? partnerData = JsonConvert.DeserializeObject<PartnerData>(json);

        if (partnerData == null)
        {
            // TODO maybe write to logs and fail forward instead of throwing exception?
            throw new Exception("Couldn't parse partner data");
        }

        // TODO only ingest the most recent partner rate for a given currency/payment/delivery combo

        _context.PartnerRates.AddRange(partnerData.PartnerRates);
        await _context.SaveChangesAsync();
    }
}

public interface IPartnerRateIngest
{
    void IngestPartnerRates();
}