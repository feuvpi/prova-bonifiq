using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class ProductService
	{
		TestDbContext _ctx;
        

        public ProductService(TestDbContext ctx)
		{
			_ctx = ctx;
        }

		public ProductList ListProducts(int page)
		{
            if (page < 1) page = 1;
            int pageSize = 10; // quantidade de itens por página
            int skip = (page - 1) * pageSize; // -- quantidade de itens a serem ignorados
            int totalCount = _ctx.Products.Count();
            bool hasNext = (skip + pageSize) < totalCount;
            var products = _ctx.Products
              .OrderBy(p => p.Id)
              .Skip(skip)
              .Take(pageSize)
              .ToList();

            return new ProductList() { HasNext = hasNext, TotalCount = totalCount, Products = products };
        }

	}
}
