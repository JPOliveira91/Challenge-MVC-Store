using Challenge_MVC_Store.Data.Models;

namespace Challenge_MVC_Store.Data.Repositories.Customers
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}