using Celsia.Interfaces;
using Celsia.Models;
using Celsia.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Celsia.Data;

namespace Celsia.Services
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BaseContext _context;
        private readonly IMapper _mapper;

        public TransactionRepository(BaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Transaction> Create(TransactionCreateViewModel transactionVM)
        {
            var transaction = _mapper.Map<Transaction>(transactionVM);
            transaction.Id = Guid.NewGuid().ToString(); // Generar un nuevo ID
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> Update(string id, TransactionUpdateViewModel transactionVM)
        {
            var transaction = await GetById(id);
            if (transaction == null)
            {
                throw new Exception("La transacción no existe.");
            }

            _mapper.Map(transactionVM, transaction);
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> Delete(string id)
        {
            var transaction = await GetById(id);
            if (transaction == null)
            {
                throw new Exception("La transacción no existe.");
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> GetById(string id)
        {
            return await _context.Transactions
                                 .Include(t => t.Platform)
                                 .Include(t => t.User)
                                 .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            return await _context.Transactions
                                 .Include(t => t.Platform)
                                 .Include(t => t.User)
                                 .ToListAsync();
        }
    }
}
