using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class ProductService : BaseService<Product>
	{
		TestDbContext _ctx;

        public ProductService(TestDbContext ctx) : base(ctx)
        {
        }

    }
}
