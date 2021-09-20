using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityCore.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(object Id);
        IQueryable<T> Query();
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(object Id);
        Task Save();
    }
}
