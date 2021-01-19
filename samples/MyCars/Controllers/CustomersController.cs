using System;
using System.Linq;
using System.Threading.Tasks;
using GraniteCore;
using GraniteCore.MVC.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyCars.Areas.Identity;
using MyCars.Domain;
using MyCars.Domain.ViewModels;
using MyCars.Services;

namespace MyCars.Controllers
{
    [Authorize]
    public class CustomersController : UserBasedController<CustomersController, GraniteCoreApplicationUser, string>
    {
        private readonly ICustomerService _customerService;

        public CustomersController(
            ICustomerService customerService,
            IGraniteMapper graniteMapper,
            ILogger<CustomersController> logger,
            UserManager<GraniteCoreApplicationUser> userManager
            ) : base(graniteMapper, logger, userManager, customerService)
        {
            _customerService = customerService;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            Logger.LogInformation("************** Loading customer index");
            var customers = GraniteMapper.Map<Customer, CustomerViewModel>(_customerService.GetAll()).ToList();
            return View(customers);
            //return View(new List<CustomerViewModel>());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerViewModel = await _customerService.GetByID(
                id.Value, 
                x => x.LastModifiedByUser
                );

            if (customerViewModel == null)
            {
                return NotFound();
            }

            return View(GraniteMapper.Map<Customer,CustomerViewModel>(customerViewModel));
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Age,InceptionDate,ID")] CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                await _customerService.Create(GraniteMapper.Map<CustomerViewModel,Customer>(customerViewModel));

                return RedirectToAction(nameof(Index));
            }
            return View(customerViewModel);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerViewModel = await _customerService.GetByID(id.Value);
            if (customerViewModel == null)
            {
                return NotFound();
            }
            return View(GraniteMapper.Map<Customer, CustomerViewModel>(customerViewModel));
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FirstName,LastName,Age,InceptionDate,ID")] CustomerViewModel customerViewModel)
        {
            if (id != customerViewModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _customerService.Update(id, GraniteMapper.Map<CustomerViewModel, Customer>(customerViewModel));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerViewModelExists(customerViewModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customerViewModel);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerViewModel = await _customerService.GetByID(id.Value);

            if (customerViewModel == null)
            {
                return NotFound();
            }

            return View(GraniteMapper.Map<Customer, CustomerViewModel>(customerViewModel));
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _customerService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerViewModelExists(Guid id)
        {
            return _customerService.GetAll().Any(e => e.ID == id);
        }
    }
}
