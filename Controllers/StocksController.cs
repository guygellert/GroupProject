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
    public class StocksController : Controller
    {
        private readonly CaveretContext _context;

        public StocksController(CaveretContext context)
        {
            _context = context;
        }

        // GET: Stocks
        public async Task<IActionResult> Index()
        {
            var caveretContext = _context.Stock.Include(s => s.product);
            return View(await caveretContext.ToListAsync());
        }

        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .Include(s => s.product)
                .FirstOrDefaultAsync(m => m.id == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            ViewData["productId"] = new SelectList(_context.Products, "Id", "productName");
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,productId,quantity")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["productId"] = new SelectList(_context.Products, "Id", "productName", stock.productId);
            return View(stock);
        }

        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["productId"] = new SelectList(_context.Products, "Id", "productName", stock.productId);
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,productId,quantity")] Stock stock)
        {
            if (id != stock.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.id))
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
            ViewData["productId"] = new SelectList(_context.Products, "Id", "productName", stock.productId);
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .Include(s => s.product)
                .FirstOrDefaultAsync(m => m.id == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _context.Stock.FindAsync(id);
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockExists(int id)
        {
            return _context.Stock.Any(e => e.id == id);
        }
        public JsonResult StocksByCategories()
        {
            var result = _context.Catagories.Include(c => c.products)
                         .Select(c => 
            new
            {
                CategoryName = c.catagorieName,
                Stocks = (from prod in _context.Products 
                          where prod.Catagories.Select( cat => cat.Id).Contains(c.Id)
                          join sto in _context.Stock on prod.Id equals sto.productId
                          select sto.quantity).Sum()
             });

            return Json(result);
        }
        public JsonResult PossibleMaxCategory()
        {
            var result = _context.Catagories.Include(c => c.products)
             .Select(c =>
new
{
    CategoryName = c.catagorieName,
    Stocks = (from prod in _context.Products
              where prod.Catagories.Select(cat => cat.Id).Contains(c.Id)
              join sto in _context.Stock on prod.Id equals sto.productId
              select prod.price * sto.quantity).Sum()
});

            return Json(result);
            //var result;
            //var result = _context.Stock.Include(p => p.product).Include(p => p.product.catagory).
            //    GroupBy(p => p.product.catagory.catagorieName).Select(p =>
            //new
            //{
            //    CategoryName = p.Key,
            //    Profit = p.ToList().Sum( x=> x.product.price)
            //});

            return Json("s");
        }
    }
}
