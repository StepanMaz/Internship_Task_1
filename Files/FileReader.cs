using Entities;
using System.Collections.Concurrent;

namespace Files
{
    public abstract class FileReader
    {
        public abstract Task<ParsingResult> ReadData(string path, CancellationToken cancellationToken);

        protected bool ValidatePaymentDetails(PaymentDetails paymentDetails)
        {
            if(string.IsNullOrEmpty(paymentDetails.First_name)) return false;
            if(string.IsNullOrEmpty(paymentDetails.Last_name)) return false;
            if(string.IsNullOrEmpty(paymentDetails.Address)) return false;
            if(string.IsNullOrEmpty(paymentDetails.Service)) return false;
            return true;
        }
    }
}