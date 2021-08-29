using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Caveret.Data;
using Caveret.Models;
using Microsoft.AspNetCore.Authorization;

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
            if (!User.IsInRole("Admin"))
            {

                return View("../Home/Unauthorize");
            }
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
            //var res = _context.Products
            //           .Join(_context.Stock,
            //                prod => prod.Id,
            //                sto => sto.productId,
            //                (prod,sto) => new
            //                {
            //                    ProductId = prod.Id,
            //                    StockId = sto.id
            //                })
            //           .Join(_context.Catagories,
            //                 prod => prod.ProductId,
            //                 cat => cat.)
            //var res = _context.Stock.Include(sto => sto.product)
            //          .Join(_context.Products,
            //                 sto => sto.productId,
            //                 prod => prod.Id,
            //                 (sto, prod) => new
            //                 {
            //                     StockId = sto.id,
            //                     ProductId = prod.Id
            //                 }
            //                 )
            //          .Join(_context.Catagories,
            //                prod => prod.ProductId
            //                cat => cat.Id
            //                (prod,cat) => new 
            //                {
            //                    ProductId = cat.
            //                }
            //                )
            //          .Select(sto => sto);

            //var result = _context.Catagories.Include(c => c.products)
            //             .Select(c =>
            //new
            //{
            //    CategoryName = c.catagorieName,
            //    Stocks = (from prod in _context.Products
            //              where prod.Catagories.Select(cat => cat.Id).Contains(c.Id)
            //              join sto in _context.Stock on prod.Id equals sto.productId
            //              select sto.quantity).Sum()
            //});

            var result = _context.Catagories.Include(cat => cat.products)
                .GroupBy(cat => cat.catagorieName)
                .Select(cat => 
                new 
                {
                    CategoryName = cat.Key,
                    Stocks = (from prod in _context.Products
                          where prod.Catagories.Select(cl => cl.catagorieName).Contains(cat.Key)
                          join sto in _context.Stock on prod.Id equals sto.productId
                          select sto.quantity).Sum()
                }
                )
                ;

            result = result.Where(res => res.Stocks > 0);
            return Json(result);
        }
        public JsonResult PossibleMaxCategory()
        {
            //var result = _context.Catagories.Include(c => c.products)
            // .Select(c =>
            //    new
            //    {
            //    CategoryName = c.catagorieName,
            //    Stocks = (from prod in _context.Products
            //              where prod.Catagories.Select(cat => cat.Id).Contains(c.Id)
            //              join sto in _context.Stock on prod.Id equals sto.productId
            //              select prod.price * sto.quantity).Sum()
            //    });


            var result = _context.Catagories.Include(cat => cat.products)
    .GroupBy(cat => cat.catagorieName)
             .Select(cat =>
                new
                {
                    CategoryName = cat.Key,
                    Stocks = (from prod in _context.Products
                              where prod.Catagories.Select(c => c.catagorieName).Contains(cat.Key)
                              join sto in _context.Stock on prod.Id equals sto.productId
                              select prod.price * sto.quantity).Sum()
                });

            result = result.Where(res => res.Stocks > 0);
            return Json(result);
            //var result;
            //var result = _context.Stock.Include(p => p.product).Include(p => p.product.catagory).
            //    GroupBy(p => p.product.catagory.catagorieName).Select(p =>
            //new
            //{
            //    CategoryName = p.Key,
            //    Profit = p.ToList().Sum( x=> x.product.price)
            //});

        }
    }
}
