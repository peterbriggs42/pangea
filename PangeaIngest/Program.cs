using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Pangea.Models;

namespace Pangea.Ingest;

// TODO
// 1. ingest the data, some sort of command-line executeable that will:
//  1.1. read the file as JSON 
//  1.2. iterate over the array and validate each entry in PartnerRates array 
//       data type validation -- some sort of schema? 
//       sort the entries by AcquiredDate and only save the most recent one for a given (Currency, PaymentMethod, DeliveryMethod) combo
//  1.3. probably a csproj that's separate from the PangeaApi project
//  1.4. stick it in an in-memory database 
//  1.5. Makefile that'll have a command to ingest the file (wipe the previous entry from db before doing this)

class Program
{
    const string PARTNER_RATES_FILE_PATH = "../partner-data.json";

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var rates = IngestPartnerRates();
        Console.WriteLine("goodbyeeee");
    }

    public static List<PartnerRate>? IngestPartnerRates()
    {
        using StreamReader reader = new(PARTNER_RATES_FILE_PATH);
        var json = reader.ReadToEnd();
        PartnerData? partnerData = JsonConvert.DeserializeObject<PartnerData>(json);

        return partnerData?.PartnerRates;
    }
}
