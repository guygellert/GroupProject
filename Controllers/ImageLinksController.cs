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
    public class ImageLinksController : Controller
    {
        private readonly CaveretContext _context;

        public ImageLinksController(CaveretContext context)
        {
            _context = context;
        }

        // GET: ImageLinks
        public async Task<IActionResult> Index()
        {
            if (!User.IsInRole("Admin"))
            {

                return View("../Home/Unauthorize");
            }
            return View(await _context.ImageLink.ToListAsync());
        }

        // GET: ImageLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!User.IsInRole("Admin"))
            {

                return View("../Home/Unauthorize");
            }
            if (id == null)
            {
                return NotFound();
            }

            var imageLink = await _context.ImageLink
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageLink == null)
            {
                return NotFound();
            }

            return View(imageLink);
        }

        // GET: ImageLinks/Create
        public IActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {

                return View("../Home/Unauthorize");
            }
            return View();
        }

        // POST: ImageLinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create([Bind("Id,Address")] ImageLink imageLink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imageLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imageLink);
        }

        // GET: ImageLinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.IsInRole("Admin"))
            {

                return View("../Home/Unauthorize");
            }
            if (id == null)
            {
                return NotFound();
            }

            var imageLink = await _context.ImageLink.FindAsync(id);
            if (imageLink == null)
            {
                return NotFound();
            }
            return View(imageLink);
        }

        // POST: ImageLinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address")] ImageLink imageLink)
        {
            if (id != imageLink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageLinkExists(imageLink.Id))
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
            return View(imageLink);
        }

        // GET: ImageLinks/Delete/5

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

            var imageLink = await _context.ImageLink
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageLink == null)
            {
                return NotFound();
            }

            return View(imageLink);
        }

        // POST: ImageLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageLink = await _context.ImageLink.FindAsync(id);
            _context.ImageLink.Remove(imageLink);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageLinkExists(int id)
        {
            return _context.ImageLink.Any(e => e.Id == id);
        }
    }
}
