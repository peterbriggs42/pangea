using Newtonsoft.Json;
using Pangea.Models;

namespace Pangea.Api.Services
{
    public class PartnerRateIngestService : IPartnerRateIngestService
    {
        // TODO if I had more time I'd implement logging. We'd want to log e.g. if the service can't find the file, or runs into errors during parsing
        // private readonly ILogger<PartnerRateIngestService> _logger;

        private PangeaContext _context;
        public PartnerRateIngestService(PangeaContext context)
        {
            _context = context;
        }

        // TODO this string should be stored in config so that the location can be modified without a code change
        const string PARTNER_RATES_FILE_PATH = "../partner-data.json";

        // TODO in order to unit test this method we'd want to factor out the file read to a wrapper method
        public async void IngestPartnerRates()
        {
            using StreamReader reader = new(PARTNER_RATES_FILE_PATH);
            var json = reader.ReadToEnd();

            // TODO would be better to write to logs and fail forward instead of throwing exception
            PartnerData? partnerData = JsonConvert.DeserializeObject<PartnerData>(json) ?? throw new Exception("Couldn't parse partner data");

            // TODO only ingest the most recent partner rate for a given currency/payment/delivery combo
            _context.PartnerRates.AddRange(partnerData.PartnerRates);
            await _context.SaveChangesAsync();
        }
    }

    public interface IPartnerRateIngestService
    {
        void IngestPartnerRates();
    }
}