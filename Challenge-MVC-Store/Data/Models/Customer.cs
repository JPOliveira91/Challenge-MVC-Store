using System.ComponentModel.DataAnnotations;

namespace Challenge_MVC_Store.Data.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Por favor, insira um email válido.")]
        public required string Email { get; set; }

        public DateTime CreationDate { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}