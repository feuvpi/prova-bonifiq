using ProvaPub.Interfaces;
using ProvaPub.Models;
using static ProvaPub.Models.PaymentResult;

namespace ProvaPub.Services
{
	public class OrderService
	{
        public async Task<Order> PayOrder(IPaymentMethod paymentMethod, decimal paymentValue, int customerId)
		{
            PaymentResult paymentResult = paymentMethod.Pay(paymentValue);

            return await Task.FromResult(new Order()
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now,
                Value = paymentValue,
                PaymentResult = paymentResult
            });
		}
	}
}
