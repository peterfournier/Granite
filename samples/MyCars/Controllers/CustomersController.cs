using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyCars.Data;
using MyCars.Domain.DTOs;
using MyCars.Domain.ViewModels;
using MyCars.Services;

namespace MyCars.Controllers
{
    [Authorize]
    public class CustomersController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IGraniteMapper _mapper;

        public CustomersController(
            ICustomerService customerService,
            IGraniteMapper mapper
            )
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {         
            return View(_mapper.Map<CustomerDTO, CustomerViewModel>(_customerService.GetAll()));
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerViewModel = await _customerService.GetById(id.Value);

            if (customerViewModel == null)
            {
                return NotFound();
            }

            return View(customerViewModel);
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
                await _customerService.Create(_mapper.Map<CustomerViewModel,CustomerDTO>(customerViewModel), UserId);

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

            var customerViewModel = await _customerService.GetById(id.Value);
            if (customerViewModel == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<CustomerDTO, CustomerViewModel>(customerViewModel));
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
                    await _customerService.Update(id, _mapper.Map<CustomerViewModel, CustomerDTO>(customerViewModel), ApplicationUser.Id);
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

            var customerViewModel = await _customerService.GetById(id.Value);

            if (customerViewModel == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<CustomerDTO, CustomerViewModel>(customerViewModel));
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _customerService.Delete(id, UserId);
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerViewModelExists(Guid id)
        {
            return _customerService.GetAll().Any(e => e.ID == id);
        }
    }
}
