using Celsia.Models;
using Celsia.ViewModels;

namespace Celsia.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> Create(TransactionCreateViewModel transactionVM);
        Task<Transaction> Update(string id, TransactionUpdateViewModel transactionVM);
        Task<Transaction> Delete(string id);
        Task<Transaction> GetById(string id);
        Task<IEnumerable<Transaction>> GetAll();
    }
}
