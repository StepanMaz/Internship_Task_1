namespace Entities
{
    public class PaymentDetails
    {
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Address { get; set; }
        public decimal Payment { get; set; }
        public DateTime Date { get; set; }
        public long Account_number { get; set; }
        public string Service { get; set; }
    }
}