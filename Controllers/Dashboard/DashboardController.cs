using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Celsia.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using OfficeOpenXml;
using Celsia.Data;
using System.Data;
using Newtonsoft.Json;

namespace Celsia.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly BaseContext _context;

        public DashboardController(ILogger<DashboardController> logger, BaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Upload()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Por favor seleccione un archivo válido.";
                return View();
            }

            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Normalización del archivo Excel y guardado en la base de datos
            var tables = NormalizeAndSaveToDatabase(filePath);

            // Guardar las tablas normalizadas en TempData
            TempData["NormalizedTables"] = JsonConvert.SerializeObject(tables);

            return RedirectToAction("Preview");
        }

        private Dictionary<string, DataTable> NormalizeAndSaveToDatabase(string filePath)
        {
            var tables = new Dictionary<string, DataTable>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];  // Asumiendo que solo hay una hoja en el Excel

                // 1. Normalización de la tabla `Users`
                var usersTable = CreateUserTable(worksheet);
                SaveUsersToDatabase(usersTable);
                tables["Users"] = usersTable;

                // 2. Normalización de la tabla `Platforms`
                var platformsTable = CreatePlatformsTable(worksheet);
                SavePlatformsToDatabase(platformsTable);
                tables["Platforms"] = platformsTable;

                // 3. Normalización de la tabla `Transactions`
                var transactionsTable = CreateTransactionsTable(worksheet, platformsTable);
                SaveTransactionsToDatabase(transactionsTable);
                tables["Transactions"] = transactionsTable;

                // 4. Normalización de la tabla `Invoices`
                var invoicesTable = CreateInvoicesTable(worksheet);
                SaveInvoicesToDatabase(invoicesTable);
                tables["Invoices"] = invoicesTable;
            }

            return tables;
        }

        private DataTable CreateUserTable(ExcelWorksheet worksheet)
        {
            var table = new DataTable("Users");

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("Phone", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Password", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("CreateAt", typeof(DateTime));
            table.Columns.Add("UpdateAt", typeof(DateTime));

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                int id;
                if (!int.TryParse(worksheet.Cells[row, 7].Text, out id))
                {
                    throw new FormatException($"El valor '{worksheet.Cells[row, 7].Text}' en la fila {row} no es un ID válido.");
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword("test");  // Hash de la contraseña

                table.Rows.Add(
                    id,  // Número de Identificación
                    worksheet.Cells[row, 6].Text,  // Nombre del Cliente
                    worksheet.Cells[row, 8].Text,  // Dirección
                    worksheet.Cells[row, 9].Text,  // Teléfono
                    worksheet.Cells[row, 10].Text,  // Correo Electrónico
                    passwordHash,  // Contraseña encriptada
                    worksheet.Cells[row, 12].Text,  // Estado
                    DateTime.Now,  // Fecha de Creación
                    DateTime.Now   // Fecha de Actualización
                );
            }

            return table.DefaultView.ToTable(true, "Id", "Name", "Address", "Phone", "Email"); // Eliminar duplicados
        }

        private DataTable CreatePlatformsTable(ExcelWorksheet worksheet)
        {
            var table = new DataTable("Platforms");

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));

            var platforms = worksheet.Cells[2, 11, worksheet.Dimension.End.Row, 11].Select(cell => cell.Text).Distinct().ToList();

            for (int i = 0; i < platforms.Count; i++)
            {
                table.Rows.Add(i + 1, platforms[i]);
            }

            return table;
        }

        private DataTable CreateTransactionsTable(ExcelWorksheet worksheet, DataTable platformsTable)
        {
            var table = new DataTable("Transactions");

            table.Columns.Add("Id", typeof(string));
            table.Columns.Add("DateTime", typeof(DateTime));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("UserId", typeof(string));
            table.Columns.Add("PlatformId", typeof(int));

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var platformName = worksheet.Cells[row, 11].Text;
                var platformId = platformsTable.AsEnumerable()
                    .First(p => p.Field<string>("Name") == platformName).Field<int>("Id");

                table.Rows.Add(
                    worksheet.Cells[row, 1].Text,  // ID de la Transacción
                    DateTime.Parse(worksheet.Cells[row, 2].Text),  // Fecha y Hora de la Transacción
                    decimal.Parse(worksheet.Cells[row, 3].Text),  // Monto de la Transacción
                    worksheet.Cells[row, 4].Text,  // Estado de la Transacción
                    worksheet.Cells[row, 5].Text,  // Tipo de Transacción
                    worksheet.Cells[row, 7].Text,  // Número de Identificación (UserId)
                    platformId
                );
            }

            return table;
        }

        private DataTable CreateInvoicesTable(ExcelWorksheet worksheet)
        {
            var table = new DataTable("Invoices");

            table.Columns.Add("Number", typeof(string));
            table.Columns.Add("TransactionId", typeof(string));
            table.Columns.Add("Period", typeof(string));
            table.Columns.Add("BilledAmount", typeof(decimal));
            table.Columns.Add("PaidAmount", typeof(decimal));

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                table.Rows.Add(
                    worksheet.Cells[row, 12].Text,  // Número de Factura
                    worksheet.Cells[row, 1].Text,  // ID de la Transacción
                    worksheet.Cells[row, 13].Text,  // Periodo de Facturación
                    decimal.Parse(worksheet.Cells[row, 14].Text),  // Monto Facturado
                    decimal.Parse(worksheet.Cells[row, 15].Text)  // Monto Pagado
                );
            }

            return table.DefaultView.ToTable(true, "Number", "TransactionId", "Period", "BilledAmount", "PaidAmount"); // Eliminar duplicados
        }

        private void SaveUsersToDatabase(DataTable usersTable)
        {
            foreach (DataRow row in usersTable.Rows)
            {
                var user = new User
                {
                    Id = row["Id"].ToString(),
                    Name = row["Name"].ToString(),
                    Address = row["Address"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"].ToString(),
                    // Aquí podrías agregar la contraseña y otros campos si es necesario.
                };

                if (!_context.Users.Any(u => u.Id == user.Id))
                {
                    _context.Users.Add(user);
                }
            }
            _context.SaveChanges();
        }

        private void SavePlatformsToDatabase(DataTable platformsTable)
        {
            foreach (DataRow row in platformsTable.Rows)
            {
                var platform = new Platform
                {
                    Id = (int)row["Id"],
                    Name = row["Name"].ToString(),
                };

                if (!_context.Platforms.Any(p => p.Id == platform.Id))
                {
                    _context.Platforms.Add(platform);
                }
            }
            _context.SaveChanges();
        }

        private void SaveTransactionsToDatabase(DataTable transactionsTable)
        {
            foreach (DataRow row in transactionsTable.Rows)
            {
                var transaction = new Transaction
                {
                    Id = row["Id"].ToString(),
                    DateTime = DateTime.Parse(row["DateTime"].ToString()!),
                    Amount = decimal.Parse(row["Amount"].ToString()!),
                    Status = row["Status"].ToString(),
                    Type = row["Type"].ToString(),
                    UserId = row["UserId"].ToString(),
                    PlatformId = (int)row["PlatformId"],
                };

                if (!_context.Transactions.Any(t => t.Id == transaction.Id))
                {
                    _context.Transactions.Add(transaction);
                }
            }
            _context.SaveChanges();
        }

        private void SaveInvoicesToDatabase(DataTable invoicesTable)
        {
            foreach (DataRow row in invoicesTable.Rows)
            {
                var invoice = new Invoice
                {
                    Number = row["Number"].ToString(),
                    TransactionId = row["TransactionId"].ToString(),
                    Period = row["Period"].ToString(),
                    BilledAmount = decimal.Parse(row["BilledAmount"].ToString()!),
                    PaidAmount = decimal.Parse(row["PaidAmount"].ToString()!),
                };

                if (!_context.Invoices.Any(i => i.Number == invoice.Number))
                {
                    _context.Invoices.Add(invoice);
                }
            }
            _context.SaveChanges();
        }

        [HttpGet]
        public IActionResult Preview()
        {
            // Recuperar el JSON de TempData
            var jsonTables = TempData["NormalizedTables"] as string;

            if (string.IsNullOrEmpty(jsonTables))
            {
                ViewBag.Message = "No hay tablas disponibles para previsualizar.";
                return View();
            }

            // Deserializar el JSON a un diccionario de tablas
            var tables = JsonConvert.DeserializeObject<Dictionary<string, DataTable>>(jsonTables);

            return View(tables);
        }
    }
}

