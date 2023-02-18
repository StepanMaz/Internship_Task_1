using Entities;
using Entities.Data;
using System.Text.Json;

namespace Application
{
    public interface IDataShaper
    {
        public string TransformData(IEnumerable<PaymentDetails> paymentDetails);
    }

    public class DataShaper : IDataShaper
    {
        public string TransformData(IEnumerable<PaymentDetails> paymentDetails)
        {
            var parsedData = paymentDetails
                .GroupBy(pd => pd.Address.Split(',', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())
                .Select(c => new CityData() {
                    City = c.Key,
                    Services = c.GroupBy(pd => pd.Service).Select(s => 
                        new ServiceData() 
                            {
                                Name = s.Key,
                                Payers = s.Select(payer =>
                                    new PayerData()
                                    {
                                        Name = payer.First_name + " " + payer.Last_name,
                                        Payment = payer.Payment,
                                        Account_number = payer.Account_number,
                                        Date = payer.Date
                                    }
                                ).ToList()
                            }
                        ).ToList()
                });
            return JsonSerializer.Serialize(parsedData, options: new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        }
    }
}