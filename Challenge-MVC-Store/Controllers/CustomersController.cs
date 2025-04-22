using Challenge_MVC_Store.Data.Models;
using Challenge_MVC_Store.Data.Repositories.Customers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            TempData["ErrorMessage"] = "Dados inválidos. Verifique os campos.";
            return View(customer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}