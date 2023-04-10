﻿using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class CustomerService
    {
        TestDbContext _ctx;

        public CustomerService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        public CustomerList ListCustomers(int page)
        {
            if(page < 1) page = 1; 
            int pageSize = 10; // quantidade de itens por página
            int skip = (page - 1) * pageSize; // -- quantidade de itens a serem ignorados
            int totalCount = _ctx.Customers.Count();
            bool hasNext = (skip + pageSize) < totalCount;
            var products = _ctx.Customers
              .OrderBy(p => p.Id)
              .Skip(skip)
              .Take(pageSize)
              .ToList();

            return new CustomerList() { HasNext = hasNext, TotalCount = totalCount, Customers = products };
        }

        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            if (customerId <= 0) throw new ArgumentOutOfRangeException(nameof(customerId));

            if (purchaseValue <= 0) throw new ArgumentOutOfRangeException(nameof(purchaseValue));

            //Business Rule: Non registered Customers cannot purchase
            var customer = await _ctx.Customers.FindAsync(customerId);
            if (customer == null) throw new InvalidOperationException($"Customer Id {customerId} does not exists");

            //Business Rule: A customer can purchase only a single time per month
            var baseDate = DateTime.UtcNow.AddMonths(-1);
            var ordersInThisMonth = await _ctx.Customers.CountAsync(s => s.Id == customerId && s.Orders.Any(w => w.OrderDate >= baseDate));
            if (ordersInThisMonth > 1)
                return false;

            //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
            var haveBoughtBefore = await _ctx.Customers.CountAsync(s => s.Id == customerId && s.Orders.Any());
            if (haveBoughtBefore == 0 && purchaseValue > 100)
                return false;

            return true;
        }

    }
}
