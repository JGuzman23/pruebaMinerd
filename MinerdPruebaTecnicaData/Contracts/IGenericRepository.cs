using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MinerdPruebaTecnica.Data.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<bool> CreateAsync(T entity);
        Task<bool> EditAsync(T entity);
        Task<bool> RemoveAsync(T entity);
    }
}
