using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dir_Struct.Data;
using Dir_Struct.Models;

namespace Dir_Struct.Controllers
{
    public class FoldersController : Controller
    {
        private readonly FolderContext _context;

        public FoldersController(FolderContext context)
        {
            _context = context;
        }

        // GET: Folders
        public async Task<IActionResult> Index()
        {
              return _context.Folder_Entities != null ? 
                          View(await _context.Folder_Entities.ToListAsync()) :
                          Problem("Entity set 'FolderContext.Folder_Entities'  is null.");
        }

        // GET: Folders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Folder_Entities == null)
            {
                return NotFound();
            }

            var folder_Entity = await _context.Folder_Entities
                .FirstOrDefaultAsync(m => m.ID == id);
            if (folder_Entity == null)
            {
                return NotFound();
            }

            return View(folder_Entity);
        }

        // GET: Folders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Folders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,OwnerID")] Folder_Entity folder_Entity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(folder_Entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(folder_Entity);
        }

        // GET: Folders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Folder_Entities == null)
            {
                return NotFound();
            }

            var folder_Entity = await _context.Folder_Entities.FindAsync(id);
            if (folder_Entity == null)
            {
                return NotFound();
            }
            return View(folder_Entity);
        }

        // POST: Folders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,OwnerID")] Folder_Entity folder_Entity)
        {
            if (id != folder_Entity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(folder_Entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Folder_EntityExists(folder_Entity.ID))
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
            return View(folder_Entity);
        }

        // GET: Folders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Folder_Entities == null)
            {
                return NotFound();
            }

            var folder_Entity = await _context.Folder_Entities
                .FirstOrDefaultAsync(m => m.ID == id);
            if (folder_Entity == null)
            {
                return NotFound();
            }

            return View(folder_Entity);
        }

        // POST: Folders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Folder_Entities == null)
            {
                return Problem("Entity set 'FolderContext.Folder_Entities'  is null.");
            }
            var folder_Entity = await _context.Folder_Entities.FindAsync(id);
            if (folder_Entity != null)
            {
                _context.Folder_Entities.Remove(folder_Entity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Folder_EntityExists(int id)
        {
          return (_context.Folder_Entities?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
