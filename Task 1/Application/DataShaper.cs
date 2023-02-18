using Entities;
using Entities.Data;

namespace Application
{
    public interface IDataShaper
    {
        public object TransformData(PaymentDetails paymentDetails);
    }
}