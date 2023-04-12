using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;
using Xunit;

namespace ProvaPub.Tests
{
    public class CustomerServiceTests
    {
        private readonly DbContextOptions<TestDbContext> _options;

        public CustomerServiceTests()
        {
            // -- configurar banco de dados de teste
            _options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "CustomerServiceTests")
                .Options;

        }

        [Fact]
        public async Task CanPurchase_Invalid_CustomerId_Exception()
        {
            // -- Arrange
            var _customerService = new CustomerService(new TestDbContext(_options));

            // -- Act
            async Task Act() => await _customerService.CanPurchase(0, 100);

            // -- Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(Act);
        }

        [Fact]
        public async Task CanPurchase_Invalid_PurchaseValue_Exception()
        {
            // -- Arrange
            var _customerService = new CustomerService(new TestDbContext(_options));

            // -- Act
            async Task Act() => await _customerService.CanPurchase(1, 0);
            async Task Act2() => await _customerService.CanPurchase(1, -1);

            // -- Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(Act);
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(Act2);
        }

        [Fact]
        public async Task CanPurchase_Customer_Not_Exist_Exception()
        {
            // -- Arrange
            var _customerService = new CustomerService(new TestDbContext(_options));

            // -- Act
            async Task Act() => await _customerService.CanPurchase(9999, 100);

            // -- Assert
            await Assert.ThrowsAsync<InvalidOperationException>(Act);
        }

        [Fact]
        public async Task CanPurchase_Customer_Purchased_More_Than_Once_In_Month()
        {
            // -- configurar db in memory
            DbContextOptions<TestDbContext> _options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "MoreThanOnceLastMonth")
                .Options;
            using var _ctx = new TestDbContext(_options);

            // -- arrange
            var _customerService = new CustomerService(_ctx);
            await _ctx.Customers.AddAsync(new Customer { Name = "José", Id = 1 });
            await _ctx.Orders.AddAsync(new Order { CustomerId = 1, OrderDate = DateTime.UtcNow.AddDays(-15) });
            await _ctx.SaveChangesAsync();

            // -- Act
            var test1 = await _customerService.CanPurchase(1, 100);

            // -- Assert
            Assert.False(test1);

            // -- Dispose context
            _ctx.Dispose();
        }

        [Fact]
        public async Task CanPurchase_Order_Value_Maximum_OneHundred()
        {
            // -- configurar instancia exclusiva
            DbContextOptions<TestDbContext> _options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "MaximumOneHundredOrder")
                .Options;

            // -- Arrange
            using var _ctx = new TestDbContext(_options);
            var _customerService = new CustomerService(_ctx);
            await _ctx.Customers.AddAsync(new Customer { Name = "José", Id = 1 });
            _ctx.SaveChanges();

            // -- Act
            var test1 = await _customerService.CanPurchase(1, 100);
            var test2 = await _customerService.CanPurchase(1, 101);

            // -- Assert
            Assert.True(test1);
            Assert.False(test2);

            // -- Dispose context
            _ctx.Dispose();
        }

        [Fact]
        public async Task CanPurchase_Should_Return_False_When_Customer_Never_Bought_Before_And_PurchaseValue_Is_More_Than_100()
        {
            // -- configurar instancia exclusiva
            DbContextOptions<TestDbContext> _options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "NeverBought_OrderMoreThan100")
                .Options;

            // -- Arrange
            using var _ctx = new TestDbContext(_options);
            var _customerService = new CustomerService(_ctx);
            await _ctx.Customers.AddAsync(new Customer { Name = "José", Id = 1 });
            _ctx.SaveChanges();

            // -- Act
            var test = await _customerService.CanPurchase(1, 200);

            // -- Assert
            Assert.False(test);

            // -- Dispose context
            _ctx.Dispose();
        }

        [Fact]
        public async Task CanPurchase_Should_Return_True_When_Customer_Never_Bought_Before_And_PurchaseValue_Is_Less_Or_Equal_To_100()
        {
            // -- configurar instancia exclusiva
            DbContextOptions<TestDbContext> _options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "NeverBought_OrderLessThan100")
                .Options;

            // -- Arrange
            using var _ctx = new TestDbContext(_options);
            var _customerService = new CustomerService(_ctx);
            await _ctx.Customers.AddAsync(new Customer { Name = "José", Id = 1 });
            _ctx.SaveChanges();

            // -- Act
            var result = await _customerService.CanPurchase(1, 100);

            // -- Assert
            Assert.True(result);

            // -- Dispose context
            _ctx.Dispose();
        }

    }
}
