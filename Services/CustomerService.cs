using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class CustomerService : BaseService<Customer>
    {
        public CustomerService(TestDbContext ctx) : base(ctx)
        {
        }

        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            if (customerId <= 0) throw new ArgumentOutOfRangeException(nameof(customerId));

            if (purchaseValue <= 0) throw new ArgumentOutOfRangeException(nameof(purchaseValue));

            //Business Rule: Non registered Customers cannot purchase
            var customer = await _dbSet.FindAsync(customerId);
            if (customer == null) throw new InvalidOperationException($"Customer Id {customerId} does not exist");

            //Business Rule: A customer can purchase only a single time per month
            var baseDate = DateTime.UtcNow.AddMonths(-1);
            var ordersInThisMonth = _ctx.Orders.Count(o => o.CustomerId == customerId && o.OrderDate >= baseDate);
            if (ordersInThisMonth > 0) // alterei esse if pois não estava de acordo com a regra do negocio comentada acima, o que tava impactando nos testes.
                return false;

            //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
            var haveBoughtBefore = _ctx.Orders.Any(o => o.CustomerId == customerId);
            if (!haveBoughtBefore && purchaseValue > 100)
                return false;

            return true;
        }

    }
}
