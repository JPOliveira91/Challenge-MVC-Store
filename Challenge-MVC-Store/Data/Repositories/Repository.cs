using Microsoft.EntityFrameworkCore;

namespace Challenge_MVC_Store.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task CreateAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _context.Entry(entity).State = EntityState.Modified;

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}