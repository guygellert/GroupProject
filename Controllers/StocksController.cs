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
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString , int? pageNumber)
        {
            ViewData["catagories"] = _context.Catagories.ToList();
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            var caveretContext = from s in _context.Stock
                                 join prod in _context.Products on s.id equals prod.Id
                                 select new Stock { product = prod , id = s.id , quantity = s.quantity , productId = s.productId};

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                caveretContext = caveretContext.Where(s => s.product.productName.Contains(searchString));
            }

            var stock = new object();
            if (!User.IsInRole("Admin"))
            {

                return View("../Home/Unauthorize");
            }

            switch (sortOrder)
            {
                case "name_desc":
                    {
                        caveretContext = caveretContext.OrderByDescending(s => s.product.productName);
                        break;
                    }
                
            }
            int pageSize = 3;
            return View(await PaginatedList<Stock>.CreateAsync(caveretContext.AsNoTracking(), pageNumber ?? 1, pageSize));
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
        public JsonResult CategoryProfit()
        {
            var productsProfit = new object();
            List<Catagories> cList = _context.Catagories.ToList();
            List<object> stats = new List<object>();
            //cList.ForEach(category =>
            ////{
            //    var products = _context.Catagories.Include(cat => cat.products)

            //       .Where(cat => (cat.Id.Equals(category) || category == null))
            //       .SelectMany(cat => cat.products);

            //List<Catagories> cList = _context.Catagories.ToList();

            cList.ForEach(category =>
            {
                var products = _context.Catagories.Include(cat => cat.products)
                  .Where(cat => (cat.Id.Equals(category.Id) || category == null))
                     .SelectMany(cat => cat.products);

                var productsProfit = products.Select(prod =>
                new 
                {
                    id = category.Id,
                    CategoryName = prod.productName,
                    Stocks = prod.price *
                            _context.ShopCartItem.Where(cart => cart.ProductsId == prod.Id).Sum(cart => cart.Quantity)

                }).ToList();
                productsProfit.ForEach(prof =>
                {
                    if(prof.Stocks > 0)
                    {
                        stats.Add(prof);
                    }
                    
                });
            });

            //var jsonPerCatgory =  stats.GroupBy(stat => stat.id);
            //});
            
            //IEnumerable<satistics> results = stats.Where(stat => stat.Stocks > 0);
            return Json(stats);
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
