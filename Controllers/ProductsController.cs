using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Caveret.Data;
using Caveret.Models;
//using Caveret.Handler;
namespace Caveret.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CaveretContext _context;

       



        public ProductsController(CaveretContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Search(int queryId, string queryName,int queryMaxPrice)
        {
            
            ViewData["catagories"] = new SelectList(_context.Catagories, nameof(Catagories.Id), nameof(Catagories.catagorieName));
            //var products = _context.Products.Include(prod => prod.Catagories)
            //    .Where(prod => (prod.productName.Contains(queryName) || queryName == null) &&
            //                   (prod.price <= queryMaxPrice || queryMaxPrice == 0)
            //                   )
            //    .Join(_context.Catagories,
            //           prod => prod.Id,
            //           cat => cat.Id,
            //           )

            var products = _context.Catagories.Include(cat => cat.products)
                           .Where(cat => (cat.Id.Equals(queryId) || queryId == null))
                           .SelectMany(cat => cat.products);

            products = products.Where(prod => (prod.productName.Contains(queryName) || queryName == null) &&
                                              (prod.price <= queryMaxPrice || queryMaxPrice == 0))
                                .Select(prod => prod);



            return View("Index" ,  products);
        }


        // GET: Products
        public async Task<IActionResult> Index()
        {
            
            ViewData["catagories"] = new SelectList(_context.Catagories, nameof(Catagories.Id), nameof(Catagories.catagorieName));

            var prod = from p in _context.Products
                                  join s in _context.Stock on p.Id equals s.productId
                                  where s.quantity >= 0
                                  select p;
            return View(await prod.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["catagories"] = new SelectList(_context.Catagories,nameof(Catagories.Id),nameof(Catagories.catagorieName));
            
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,productName,price,description,imgUrl")] Products products , int [] Catagories)
        {
            if (ModelState.IsValid)
            {
                //Twitter t = new Twitter();
                //t.SendText("First Test Yay");
                products.Catagories = new List<Catagories>();
                products.Catagories.AddRange(_context.Catagories.Where(x => Catagories.Contains(x.Id)));
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["catagories"] = new SelectList(_context.Catagories, nameof(Catagories.Id), nameof(Catagories.catagorieName));
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,productName,price,description,imgUrl")] Products products, int[] Catagories)
        {
            if (id != products.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    products.Catagories = new List<Catagories>();
                    products.Catagories.AddRange(_context.Catagories.Where(x => Catagories.Contains(x.Id)));
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
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
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
