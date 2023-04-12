namespace ProvaPub.Models
{
    public class ItemList<IBaseEntity>
    {
        public List<IBaseEntity>? Items { get; set; }
        public int TotalCount { get; set; }
        public bool HasNext { get; set; }

    }
}
