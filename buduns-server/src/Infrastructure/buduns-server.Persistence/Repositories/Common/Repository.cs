using buduns_server.Application.Exceptions;
using buduns_server.Application.Repositories.Common;
using buduns_server.Domain.Entities.Common;
using buduns_server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Persistence.Repositories.Common
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly BudunsDbContext _context;

        public Repository(BudunsDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public virtual async Task<List<T>> GetAllAsync() => await Table.ToListAsync();

        public virtual async Task<T> GetByIdAsync(int id) => await Table.FindAsync(id);

        public async Task<EntityEntry<T>> AddAsync(T entity) => await Table.AddAsync(entity);

        public EntityEntry<T> Update(T entity) => Table.Update(entity);

        public async Task<EntityEntry<T>> DeleteAsync(int id)
        {
            var entity = await Table.FindAsync(id);
            if (entity == null)
            {
                throw new NotFoundException("Entity bulunamadý!");
            }
            var result = Table.Remove(entity);
            return result;
        }
    }
}
