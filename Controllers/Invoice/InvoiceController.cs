using Celsia.Interfaces;
using Celsia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Celsia.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceRepository _service;

        public InvoiceController(IInvoiceRepository invoiceService)
        {
            _service = invoiceService;
        }

        public async Task<IActionResult> Index()
        {
            var invoices = await _service.GetAll();
            return View(invoices);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InvoiceCreateViewModel invoiceVM)
        {
            if (ModelState.IsValid)
            {
                await _service.Create(invoiceVM);
                return RedirectToAction(nameof(Index));
            }
            return View(invoiceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string number, InvoiceUpdateViewModel invoiceVM)
        {
            if (ModelState.IsValid)
            {
                await _service.Update(number, invoiceVM);
                return RedirectToAction(nameof(Index));
            }
            return View(invoiceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string number)
        {
            await _service.Delete(number);
            return RedirectToAction(nameof(Index));
        }
    }
}
