using Celsia.Models;
using Celsia.ViewModels;

namespace Celsia.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<Invoice> Create(InvoiceCreateViewModel invoiceVM);
        Task<Invoice> Update(string number, InvoiceUpdateViewModel invoiceVM);
        Task<Invoice> Delete(string number);
        Task<Invoice> GetById(string number);
        Task<IEnumerable<Invoice>> GetAll();
    }
}