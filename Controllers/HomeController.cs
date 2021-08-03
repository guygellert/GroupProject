using Caveret.Data;
using Caveret.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Caveret.Controllers
{
    public class HomeController : Controller
    {
        private readonly CaveretContext _logger;

        public HomeController(CaveretContext logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> NavToProducts(int? id)
        {
            ViewData["catagories"] = new SelectList(_logger.Catagories, nameof(Catagories.Id), nameof(Catagories.catagorieName));
            var result = _logger.Catagories.Include(cat => cat.products)
                              .Where(cat => (cat.Id.Equals(id) || id == null))
                              .SelectMany(cat => cat.products);
            return View("../Products/Index" , result);
        }
        public JsonResult getCatagoryList()
        {
            List<Navigation> navList = new List<Navigation>();
            Navigation nav = new Navigation();
            foreach(var cat in _logger.Catagories)
            {
                nav.text = cat.catagorieName;
                nav.url = "Products";

                navList.Add(nav);
            }

            var result = _logger.Catagories.Select(c =>
            new
            {
                text = c.catagorieName,
                url = "Home/NavToProducts/" + c.Id,
                iconCss = "icon-sweets icon",
                color =  "white"
            });
            JsonResult js = Json(navList);
            js.ContentType = "Navigation";

            return Json(result);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
