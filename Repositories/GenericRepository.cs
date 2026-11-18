using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ElearningQuiz.Core.Interfaces;
using ElearningQuiz.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElearningQuiz.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ElearningQuizDbContext _context;

        public GenericRepository(ElearningQuizDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            
            var keyProperty = _context.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties.FirstOrDefault();

            if (keyProperty != null)
            {
                var keyValue = entity.GetType().GetProperty(keyProperty.Name)?.GetValue(entity);

                if (keyValue != null)
                {
                    
                    var trackedEntity = _context.Set<T>()
                        .Local
                        .FirstOrDefault(e => e.GetType().GetProperty(keyProperty.Name)?.GetValue(e)?.Equals(keyValue) == true);

                    
                    if (trackedEntity != null)
                    {
                        _context.Entry(trackedEntity).State = EntityState.Detached;
                    }
                }
            }

            
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public Task GetByIdAsync( object userId)
        {
            throw new NotImplementedException();
        }
    }
}