using ProvaPub.Interfaces;

namespace ProvaPub.Models
{
	public class Product : IBaseEntity
	{
		public string Name { get; set; }
        public int Id { get; set; }
    }
}
