using Microsoft.EntityFrameworkCore;
using MinerdPruebaTecnica.Data.Contracts;
using MinerdPruebaTecnica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinerdPruebaTecnica.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        readonly PTMContext _context;

        readonly DbSet<Account> _accounts;
        public AccountRepository(PTMContext pTMContext)
        {
            _context = pTMContext;
            _accounts = _context.Set<Account>();
        }

        public async Task<bool> CreateAsync(Account entity)
        {
            var result = false;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.AddAsync(entity);

                    result = await _context.SaveChangesAsync() > 0;

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception (ex.InnerException.Message);
                }
            }

            return result;
        }

        public async Task<bool> EditAsync(Account entity)
        {
            var result = false;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _accounts.Attach(entity);
                    _context.Entry(entity).Property(i => i.Password).IsModified = true;
                    _context.Entry(entity).Property(i => i.Email).IsModified = true;

                    result = await _context.SaveChangesAsync() > 0;

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception (ex.InnerException.Message);
                }
            }

            return result;
        }

        public async Task<IEnumerable<Account>> GetAllAsync() 
            => await _accounts.ToListAsync();

        public async Task<Account> GetByEmailAsync(string email)
            => await _accounts.FirstOrDefaultAsync(i => i.Email.ToUpper() == email.ToUpper());

        public Task<Account> GetByIdAsync(int id)
            => _accounts.FirstOrDefaultAsync(i => i.ID == id);

        public async Task<bool> IsEmailUsedAsync(string email)
            => await _accounts.AsNoTracking()
                              .FirstOrDefaultAsync(i => i.Email.ToUpper() == email.ToUpper()) != null;

        public async Task<bool> RemoveAsync(Account entity)
        {
            var result = false;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Remove(entity);

                    result = await _context.SaveChangesAsync() > 0;

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }

            return result;
        }
    }
}
