using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Project_Management.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DataContext _context;
        protected readonly DbSet<TEntity> entities;

        public Repository(DataContext context)
        {
            this._context = context;
            entities = context.Set<TEntity>();
        }

        public TEntity GetById(Guid id)
        {
            return entities.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return entities.ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return entities.Where(predicate);
        }

        public void Add(TEntity entity)
        {
            entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            entities.Update(entity);
            _context.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            entities.Remove(entity);
            _context.SaveChanges();
        }
    }
}
