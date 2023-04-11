using ProvaPub.Models;

namespace ProvaPub.Interfaces
{
    public interface IPaymentMethod
    {
        PaymentResult Pay(decimal value);
    }
}
