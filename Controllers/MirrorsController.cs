using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReflectiveMirror.Data;
using ReflectiveMirror.Models;
using Microsoft.AspNetCore.Authorization;

namespace ReflectiveMirror.Controllers
{
    public class MirrorsController : Controller
    {
        private readonly ReflectiveMirrorContext _context;

        public MirrorsController(ReflectiveMirrorContext context)
        {
            _context = context;
        }

        // GET: Mirrors
        public async Task<IActionResult> Index()
        {
              return _context.Mirror != null ? 
                          View(await _context.Mirror.ToListAsync()) :
                          Problem("Entity set 'ReflectiveMirrorContext.Mirror'  is null.");
        }

        // GET: Mirrors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mirror == null)
            {
                return NotFound();
            }

            var mirror = await _context.Mirror
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mirror == null)
            {
                return NotFound();
            }

            return View(mirror);
        }

        // GET: Mirrors/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mirrors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Height,Width,Material,Price,Shape,Type,ImageUrl")] Mirror mirror)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mirror);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mirror);
        }

        // GET: Mirrors/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mirror == null)
            {
                return NotFound();
            }

            var mirror = await _context.Mirror.FindAsync(id);
            if (mirror == null)
            {
                return NotFound();
            }
            return View(mirror);
        }

        // POST: Mirrors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Height,Width,Material,Price,Shape,Type,ImageUrl")] Mirror mirror)
        {
            if (id != mirror.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mirror);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MirrorExists(mirror.Id))
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
            return View(mirror);
        }

        // GET: Mirrors/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mirror == null)
            {
                return NotFound();
            }

            var mirror = await _context.Mirror
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mirror == null)
            {
                return NotFound();
            }

            return View(mirror);
        }

        // POST: Mirrors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mirror == null)
            {
                return Problem("Entity set 'ReflectiveMirrorContext.Mirror'  is null.");
            }
            var mirror = await _context.Mirror.FindAsync(id);
            if (mirror != null)
            {
                _context.Mirror.Remove(mirror);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MirrorExists(int id)
        {
          return (_context.Mirror?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
