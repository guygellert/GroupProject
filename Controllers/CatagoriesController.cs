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
    public class CatagoriesController : Controller
    {
        private readonly CaveretContext _context;

        public CatagoriesController(CaveretContext context)
        {
            _context = context;
        }

        // GET: Catagories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Catagories.ToListAsync());
        }

        // GET: Catagories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catagories = await _context.Catagories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catagories == null)
            {
                return NotFound();
            }

            return View(catagories);
        }

        // GET: Catagories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catagories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,catagorieName")] Catagories catagories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catagories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(catagories);
        }

        // GET: Catagories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catagories = await _context.Catagories.FindAsync(id);
            if (catagories == null)
            {
                return NotFound();
            }
            return View(catagories);
        }

        // POST: Catagories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,catagorieName")] Catagories catagories)
        {
            if (id != catagories.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catagories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatagoriesExists(catagories.Id))
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
            return View(catagories);
        }

        // GET: Catagories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catagories = await _context.Catagories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catagories == null)
            {
                return NotFound();
            }

            return View(catagories);
        }

        // POST: Catagories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catagories = await _context.Catagories.FindAsync(id);
            _context.Catagories.Remove(catagories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatagoriesExists(int id)
        {
            return _context.Catagories.Any(e => e.Id == id);
        }
    }
}
