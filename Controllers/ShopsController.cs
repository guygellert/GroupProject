using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Caveret.Data;
using Caveret.Models;

namespace Caveret.Controllers
{
    public class ShopsController : Controller
    {
        private readonly CaveretContext _context;

        public ShopsController(CaveretContext context)
        {
            _context = context;
        }

        // GET: Shops
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shops.ToListAsync());
        }

        public async Task<IActionResult> Search(string queryName, string queryAddress,bool isOpen)
        {
            var shops = from shop in _context.Shops 
                        select shop;

            shops = shops.Where(shop => ((shop.Description.Contains(queryName) || queryName == null) &&
                                        (shop.Address.Contains(queryAddress) || queryAddress == null))).Select(shop => shop);

            List<Shops> shopList = shops.ToList();
            if (isOpen == true)
            {
                
                List<Shops> shopListOpen = new List<Shops>();
                TimeSpan now = DateTime.Now.TimeOfDay;
                shopList.ForEach(sho =>
               {
                   if (sho.OpeningTime < sho.ClosingTime
                       && now >= sho.OpeningTime.TimeOfDay && now < sho.ClosingTime.TimeOfDay)
                   {
                       shopListOpen.Add(sho);
                   }
                   else if (sho.OpeningTime > sho.ClosingTime &&
                           (now >= sho.OpeningTime.TimeOfDay || now < sho.ClosingTime.TimeOfDay))
                   {
                       shopListOpen.Add(sho);
                   }
               });
                shopList = shopListOpen;
                //shops = shopListOpen();
            }

            return View("Index", shopList);
        }

        // GET: Shops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shops = await _context.Shops
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shops == null)
            {
                return NotFound();
            }

            return View(shops);
        }

        // GET: Shops/Create

        public IActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("unauthorize", "Home");
            }
            return View();
        }

        // POST: Shops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,Description,OpeningTime,ClosingTime")] Shops shops)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("unauthorize", "Home");
            }
            if (ModelState.IsValid)
            {
                Twitter t = new Twitter();
                _context.Add(shops);
                await _context.SaveChangesAsync();
                await t.SendText("New !!! Our Honey Now Come To Base: " + shops.Description + "Now Between " + shops.OpeningTime.TimeOfDay + "To : " + shops.ClosingTime.TimeOfDay);
                return RedirectToAction(nameof(Index));
            }
            return View(shops);
        }

        // GET: Shops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("unauthorize", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var shops = await _context.Shops.FindAsync(id);
            if (shops == null)
            {
                return NotFound();
            }
            return View(shops);
        }

        // POST: Shops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,Description,OpeningTime,ClosingTime")] Shops shops)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("unauthorize", "Home");
            }
            if (id != shops.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shops);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopsExists(shops.Id))
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
            return View(shops);
        }

        // GET: Shops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("unauthorize", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var shops = await _context.Shops
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shops == null)
            {
                return NotFound();
            }

            return View(shops);
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shops = await _context.Shops.FindAsync(id);
            _context.Shops.Remove(shops);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopsExists(int id)
        {
            return _context.Shops.Any(e => e.Id == id);
        }
    }
}
