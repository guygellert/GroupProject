using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Caveret.Data;
using Caveret.Models;
using System.Net.Http;
using System.Media;
using System.IO;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
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

        public ActionResult PreapreToAdd(Products prod, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            //TempData["Products"] = prod;
            RouteValueDictionary rvd = new RouteValueDictionary(prod);
            rvd.Add("quantity", quantity);
            return RedirectToAction("Add", "AddToCart", rvd);
        }
        public async void textToSpeechPost(Products p)
        {


            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://voicerss-text-to-speech.p.rapidapi.com/?key=dcd702a3ea80430e843eb1b1bcd46f73"),
                Headers =
            {
                { "x-rapidapi-key", "9cf7294f68msh8690be7ab91562cp15dafajsn651d432b79a6" },
                { "x-rapidapi-host", "voicerss-text-to-speech.p.rapidapi.com" },
            },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "f", "8khz_8bit_mono" },
                { "c", "mp3" },
                { "r", "0" },
                { "hl", "he-il" },
                { "src", p.productName },
            }),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(body);
                byte[] result = response.Content.ReadAsByteArrayAsync().Result.ToArray();
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\voice", p.Id + ".mp3");
                System.IO.MemoryStream ms = new System.IO.MemoryStream(result, true);
                FileStream fStram = System.IO.File.Create(filePath);
                var data = await response.Content.ReadAsStringAsync();
                fStram.Write(result);
                fStram.Close();
            }
        }
        public async void textToSpeeach()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://voicerss-text-to-speech.p.rapidapi.com/?key=dcd702a3ea80430e843eb1b1bcd46f73&query_auth=dcd702a3ea80430e843eb1b1bcd46f73&hl=he-il&src=%D7%A9%D7%9C%D7%95%D7%9D%20%D7%A2%D7%95%D7%9C%D7%9D&f=8khz_8bit_mono&c=mp3&r=0"),
                Headers =
    {
        { "x-rapidapi-key", "9cf7294f68msh8690be7ab91562cp15dafajsn651d432b79a6" },
        { "x-rapidapi-host", "voicerss-text-to-speech.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                var body = response.Content.ReadAsStream();

                if (body.CanSeek)
                {
                    body.Seek(0, System.IO.SeekOrigin.Begin);
                }
                //byte[] result = System.IO.File.ReadAllBytes(@"C:\WINDOWS\Media\ding.wav");
                byte[] result = response.Content.ReadAsByteArrayAsync().Result.ToArray();
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\lib", "חלב" + ".mp3");
                System.IO.MemoryStream ms = new System.IO.MemoryStream(result, true);
                FileStream fStram = System.IO.File.Create(filePath);
                var data = await response.Content.ReadAsStringAsync();
                fStram.Write(result);
                fStram.Close();
            }
        }

        public async Task<IActionResult> Search(int queryId, string queryName,int queryMaxPrice,int pageNumber)
        {
            
            ViewData["catagories"] = new SelectList(_context.Catagories, nameof(Catagories.Id), nameof(Catagories.catagorieName));
            ViewData["images"] = new SelectList(_context.ImageLink, nameof(ImageLink.Id), nameof(ImageLink.Address));
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

            
            products = from p in products
                       join s in _context.Stock on p.Id equals s.productId
                       where s.quantity >= 0
                       select p;
            products = products.Where(prod => (prod.productName.Contains(queryName) || queryName == null) &&
                                              (prod.price <= queryMaxPrice || queryMaxPrice == 0))
                                .Select(prod => prod).Include(img => img.imgUrl);


            return View("Index" ,  products);
        }


        // GET: Products
        public async Task<IActionResult> Index()
        {

            ViewData["catagories"] = new SelectList(_context.Catagories, nameof(Catagories.Id), nameof(Catagories.catagorieName));
            ViewData["images"] = new SelectList(_context.ImageLink, nameof(ImageLink.Id), nameof(ImageLink.Address));

            var caveretContext = _context.Products.Include(s => s.imgUrl);
            var prod = from p in caveretContext
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
            ViewData["CurrentFilter"] = 1;
            ViewData["LimitQuantity"] = _context.Stock.FirstOrDefault(sto => sto.productId == id)
                .quantity;
            var caveretContext = _context.Products.Include(s => s.imgUrl);
            var products = await caveretContext
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
            if (!User.IsInRole("Admin"))
            {

                return View("../Home/Unauthorize");
            }
            ViewData["catagories"] = new SelectList(_context.Catagories,nameof(Catagories.Id),nameof(Catagories.catagorieName));
            ViewData["images"] = new SelectList(_context.ImageLink, nameof(ImageLink.Id), nameof(ImageLink.Address));

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create([Bind("Id,productName,price,description,imgUrlId")] Products products , int [] Catagories, int imgUrl)
        {
            if (ModelState.IsValid)
            {
                //Twitter t = new Twitter();
                //t.SendText("First Test Yay");
                products.Catagories = new List<Catagories>();
                products.Catagories.AddRange(_context.Catagories.Where(x => Catagories.Contains(x.Id)));
                products.imgUrl = _context.ImageLink.Find(imgUrl);
                _context.Add(products);
                await _context.SaveChangesAsync();
                textToSpeechPost(products);
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
            if (!User.IsInRole("Admin"))
            {

                return View("../Home/Unauthorize");
            }
            ViewData["catagories"] = new SelectList(_context.Catagories, nameof(Catagories.Id), nameof(Catagories.catagorieName));
            ViewData["images"] = new SelectList(_context.ImageLink, nameof(ImageLink.Id), nameof(ImageLink.Address));
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            var prod = _context.Products.Where(prod => prod.Id == id).Include(prod => prod.Catagories);
            List<Catagories> catList = prod.FirstOrDefault(prod => prod.Id == id).Catagories;

            products.Catagories = catList;
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }
        //private void publishToTwitter()
        //{
        //    Twitter t = new Twitter();
        //    t.SendText("Now Today Double Trouble Product");
        //}
        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,productName,price,description,imgUrlId")] Products products, int[] Catagories, int imgUrl)
        {
            if (id != products.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    var prod = _context.Products.Include(prod => prod.Catagories)
                        .Include(prod => prod.imgUrl)
                  .Where(a => a.Id == id)
                  .First();

                    prod.imgUrl = _context.ImageLink.First(img => img.Id == imgUrl);

                    prod.price = products.price;
                    prod.productName = products.productName;
                    prod.description = products.description;

                    var count = prod.Catagories.Count;
                    for (var i = 0; i < count;)
                    {
                        prod.Catagories.Remove(prod.Catagories.ElementAt(i));
                        count--;
                    }
                    foreach (var idCat in Catagories)
                    {
                    
                        Catagories category = _context.Catagories.Where(ct => ct.Id == idCat).First();
                        prod.Catagories.Add(category);
                    }
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
            if (!User.IsInRole("Admin"))
            {

                return View("../Home/Unauthorize");
            }
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
        [Authorize(Roles = "Admin")]
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
