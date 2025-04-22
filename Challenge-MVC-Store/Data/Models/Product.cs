using System.ComponentModel.DataAnnotations.Schema;

namespace Challenge_MVC_Store.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; } = [];
    }
}