using EntityConfig.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityCore.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ConfigDBContext _context { get; }

        public Repository()
        {
            this._context = new ConfigDBContext();
        }
        public Repository(ConfigDBContext _context)
        {
            this._context = _context;
        }
        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public IQueryable<T> Query()
        {
            return _context.Set<T>();
        }
        public async Task<T> GetById(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task Insert(T obj)
        {
            _context.Set<T>().Add(obj);
            await _context.SaveChangesAsync();
        }
        public async Task Update(T obj)
        {
            _context.Set<T>().Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task Delete(object id)
        {
            T existing = _context.Set<T>().Find(id);
            _context.Set<T>().Remove(existing);
            await _context.SaveChangesAsync();
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
