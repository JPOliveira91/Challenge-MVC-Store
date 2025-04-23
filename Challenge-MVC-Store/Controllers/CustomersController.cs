using Challenge_MVC_Store.Data.Models;
using Challenge_MVC_Store.Data.Repositories.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using System.Net.Mail;

namespace Challenge_MVC_Store.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ILogger<CustomersController> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            ValidateModel(customer);

            if (ModelState.IsValid)
            {
                try
                {
                    customer.CreationDate = DateTime.Now;

                    await _customerRepository.CreateAsync(customer);
                    await _customerRepository.SaveAsync();

                    TempData["SuccessMessage"] = "Cliente salvo com sucesso!";

                    return RedirectToAction("Create");
                }
                catch
                {
                    TempData["ErrorMessage"] = "Erro ao salvar o cliente.";
                    return View(customer);
                }
            }

            TempData["ErrorMessage"] = GetFirstErrorMessage();
            return View(customer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region util

        private void ValidateModel(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                ModelState.AddModelError("Name", "O nome é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(customer.Email))
            {
                ModelState.AddModelError("Email", "O email é obrigatório.");
            }
            else if (!IsValidEmail(customer.Email))
            {
                ModelState.AddModelError("Email", "Por favor, insira um email válido.");
            }
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // This will throw FormatException if email is invalid
                var mailAddress = new MailAddress(email);

                return mailAddress.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public string GetFirstErrorMessage()
        {
            return ModelState.Values
                .SelectMany(v => v.Errors)
                .FirstOrDefault()?
                .ErrorMessage ?? "An unknown error occurred";
        }

        #endregion util
    }
}