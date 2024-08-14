using Celsia.Data;
using Celsia.Interfaces;
using Celsia.Models;
using Celsia.ViewModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Celsia.Services
{
    public class InvoiceService : IInvoiceRepository
    {
        private readonly BaseContext _context;
        private readonly IMapper _mapper;

        public InvoiceService(BaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;            
        }

        public async Task<Invoice> Create(InvoiceCreateViewModel invoiceVM)
        {
            var invoice = _mapper.Map<Invoice>(invoiceVM);
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice> Update(string number, InvoiceUpdateViewModel invoiceVM)
        {
            var invoice = await GetById(number);
            if (invoice == null)
            {
                throw new Exception("La factura no existe.");
            }
            _mapper.Map(invoiceVM, invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice> Delete(string number)
        {
            var invoice = await GetById(number);
            if (invoice == null)
            {
                throw new Exception("La factura no existe.");
            }
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice> GetById(string number)
        {
            return await _context.Invoices.FirstOrDefaultAsync(i => i.Number == number);
        }

        public async Task<IEnumerable<Invoice>> GetAll()
        {
            return await _context.Invoices.ToListAsync();
        }
    }
}
