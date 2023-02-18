namespace Entities.Data
{
    public class CityData 
    {
        public string City { get; set; }
        public List<ServiceData> Services { get; set; }
        public decimal Total { get => Services.Sum(t => t.Total); } 
    }

    public class ServiceData 
    {
        public string Name { get; set; }
        public List<PayerData> Payers { get; set; }
        public decimal Total { get => Payers.Sum(t => t.Payment); } 
    }

    public class PayerData 
    {
        public string Name { get; set; }
        public decimal Payment { get; set; }
        public DateOnly Date { get; set; }
        public long Account_number { get; set; }
    }
}