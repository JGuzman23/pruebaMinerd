using MinerdPruebaTecnica.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MinerdPruebaTecnica.Data.Contracts
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<bool> IsEmailUsedAsync(string email);
        Task<Account> GetByEmailAsync(string email);
    }
}
