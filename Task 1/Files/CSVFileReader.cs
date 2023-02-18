using Entities;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;  

namespace Files
{
    public class CSVFileReader : FileReader
    {
        public override async Task<ParsingResult> ReadData(string path, CancellationToken cancellationToken)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true
            };

            using var streamReader = File.OpenText("users.csv");
            using var csvReader = new CsvReader(streamReader, csvConfig);

            ParsingResult pr = new ParsingResult();
            pr.details = new List<PaymentDetails>();
            pr.file_path = path;

            while (csvReader.Read())
            {
                try
                {
                    var pd = new PaymentDetails()
                    {
                        First_name = csvReader.GetField<string>(0),
                        Last_name = csvReader.GetField<string>(1),
                        Address = csvReader.GetField<string>(2).Trim('"', '”', '“'),
                        Payment = decimal.Parse(csvReader.GetField<string>(3), CultureInfo.InvariantCulture),
                        Date = DateTime.ParseExact(csvReader.GetField<string>(4), "yyyy-dd-MM", CultureInfo.InvariantCulture),
                        Account_number = long.Parse(csvReader.GetField<string>(5)),
                        Service = csvReader.GetField<string>(6)
                    };
                    pr.lines++;
                }
                catch
                {
                    pr.failed_lines++;
                }
            }

            return pr;
        }
    }
}