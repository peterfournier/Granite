using System;
using System.Threading.Tasks;
using GraniteCore;
using GraniteCore.MVC.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyCars.Domain.DTOs;
using MyCars.Domain.ViewModels;
using MyCars.Services;

namespace MyCars.Controllers
{
    [AllowAnonymous]
    public class CarsController : BaseController<CarsController>
    {
        private readonly ICarService _carService;

        public CarsController(
            ICarService carService,
            IGraniteMapper graniteMapper,
            ILogger<CarsController> logger
            ) : base (graniteMapper, logger)
        {
            _carService = carService;
        }

        // GET: Cars
        public IActionResult Index()
        {
            return View(_carService.GetTopCars(5));
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carEntity = await _carService.GetById(id.Value);
            if (carEntity == null)
            {
                return NotFound();
            }

            return View(carEntity);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Year,ColorHex,Make,Model,ID")] CarViewModel carViewModel)
        {
            if (ModelState.IsValid)
            {
                await _carService.Create(GraniteMapper.Map<CarViewModel, CarDTO>(carViewModel));
                return RedirectToAction(nameof(Index));
            }
            return View(carViewModel);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carEntity = await _carService.GetById(id.Value);
            if (carEntity == null)
            {
                return NotFound();
            }
            return View(carEntity);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Year,ColorHex,Make,Model,ID")] CarViewModel carViewModel)
        {
            if (id != carViewModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _carService.Update(id, GraniteMapper.Map<CarViewModel, CarDTO>(carViewModel));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarEntityExists(carViewModel.ID))
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
            return View(carViewModel);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carEntity = await _carService.GetById(id.Value);
            if (carEntity == null)
            {
                return NotFound();
            }

            return View(carEntity);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _carService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CarEntityExists(Guid id)
        {
            return _carService.GetById(id) != null;
        }
    }
}
