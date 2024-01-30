
namespace Product.Domain.DomainEntities
{
    public class Product
    {
        public int Id { get; init; } 
        public  string Name { get; init; }
        public long Qty { get; init; }
        public Product(int id,string name,long qty) 
        {
            Id = id;
            Name = name;
            Qty = qty;
        }
    }
}
