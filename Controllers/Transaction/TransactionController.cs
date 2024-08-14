using Celsia.Interfaces;
using Celsia.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Celsia.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _service;

        public TransactionController(ITransactionRepository transactionService)
        {
            _service = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _service.GetAll();
            return View(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(TransactionCreateViewModel transactionVM)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(transactionVM.Id))
                {
                    // Si el Id está vacío, crear una nueva transacción
                    transactionVM.Id = Guid.NewGuid().ToString(); // Genera un nuevo Id
                    await _service.Create(transactionVM);
                }
                else
                {
                    // Si el Id tiene valor, actualizar la transacción existente
                    var updateVM = new TransactionUpdateViewModel
                    {
                        DateTime = transactionVM.DateTime,
                        Amount = transactionVM.Amount,
                        Status = transactionVM.Status,
                        Type = transactionVM.Type,
                        PlatformId = transactionVM.PlatformId
                    };

                    await _service.Update(transactionVM.Id, updateVM);
                }

                return Json(new { success = true });
            }
            
            return Json(new { success = false, message = "Datos inválidos" });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
