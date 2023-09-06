namespace MS.IConstruye.Domain.Aggregates
{
    public class Product
    {
        public Product(
            int id,
            string name,
            string description,
            decimal precio,
            int stock
            )
        {
            Id = id;
            Name = name;        
            Description = description;  
            Precio = precio;
            Stock = stock;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}
