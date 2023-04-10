using ProvaPub.Interfaces;

namespace ProvaPub.Models
{
	public class Customer : IBaseEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<Order> Orders { get; set; }
	}
}
