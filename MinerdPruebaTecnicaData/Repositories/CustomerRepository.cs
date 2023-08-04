using Microsoft.EntityFrameworkCore;
using MinerdPruebaTecnica.Data.Contracts;
using MinerdPruebaTecnica.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MinerdPruebaTecnica.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        PTMContext _context;
        DbSet<Customer> _customers;
        public CustomerRepository(PTMContext context)
        {
            _context = context;
            _customers = context.Set<Customer>();
        }
        public async Task<bool> CreateAsync(Customer entity)
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
                    throw new Exception(ex.Message);
                }
            }

            return result;
        }

        public async Task<bool> EditAsync(Customer entity)
        {
            var result = false;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _customers.Attach(entity);

                    _context.Entry(entity).State = EntityState.Modified;

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

        public async Task<IEnumerable<Customer>> GetAllAsync()
            => await _customers.ToListAsync();

        public async Task<Customer> GetByIdAsync(int id)
            => await _customers.FirstOrDefaultAsync(i => i.ID == id);

        public async Task<bool> RemoveAsync(Customer entity)
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
