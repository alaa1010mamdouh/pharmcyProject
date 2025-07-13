using Microsoft.EntityFrameworkCore;
using pharmcy.Repository.Data;
using Pharmcy.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace pharmcy.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly PharmcyContext _context;

        public GenericRepository(PharmcyContext context)
        {
            _context = context;
        }
     
        public async Task AddAsync(T entity)
        => await   _context.Set<T>().AddAsync(entity);
        /// ////////////////////////////////////
        public void Delete(T entity)  
          =>  _context.Remove(entity);
        

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
           return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        
         => await  _context.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
       
         => await  _context.Set<T>().FindAsync(id);

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
