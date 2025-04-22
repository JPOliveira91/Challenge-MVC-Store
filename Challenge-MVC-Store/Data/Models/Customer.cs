namespace Challenge_MVC_Store.Data.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}