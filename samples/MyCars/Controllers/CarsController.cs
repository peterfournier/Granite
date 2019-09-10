using System;
using System.Threading.Tasks;
using GraniteCore;
using Microsoft.AspNetCore.Mvc;
using MyCars.Domain.DTOs;
using MyCars.Domain.ViewModels;
using MyCars.Services;

namespace MyCars.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService _carService;
        private readonly IGraniteMapper _mapper;

        public CarsController(
            ICarService carService,
            IGraniteMapper mapper
            )
        {
            _carService = carService;
            _mapper = mapper;
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
                await _carService.Create(_mapper.Map<CarViewModel, CarDTO>(carViewModel), Guid.NewGuid());
                return RedirectToAction(nameof(Index));
            }
            return View(carViewModel);
        }

        // GET: Cars/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var carEntity = await _carService.Cars.FindAsync(id);
        //    if (carEntity == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(carEntity);
        //}

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Year,ColorHex,Make,Model,ID")] CarEntity carEntity)
        //{
        //    if (id != carEntity.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _carService.Update(carEntity);
        //            await _carService.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CarEntityExists(carEntity.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(carEntity);
        //}

        // GET: Cars/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var carEntity = await _carService.Cars
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (carEntity == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(carEntity);
        //}

        // POST: Cars/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var carEntity = await _carService.Cars.FindAsync(id);
        //    _carService.Cars.Remove(carEntity);
        //    await _carService.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CarEntityExists(Guid id)
        //{
        //    return _carService.Cars.Any(e => e.ID == id);
        //}
    }
}
