using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dir_Struct.Data;
using Dir_Struct.Models;
using System.Text;

namespace Dir_Struct.Controllers
{
    public class FoldersController : Controller
    {
        private readonly FolderContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FoldersController(FolderContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Folders
        public async Task<IActionResult> Index()
        {
            return _context.Folder_Entities != null ?
                        View(await _context.Folder_Entities.ToListAsync()) :
                        Problem("Entity set 'FolderContext.Folder_Entities'  is null.");
        }

        // GET: Folders/Folder
        public IActionResult Folder(int? id)
        {
            if (id == null || id <= 0)
                id = 1;

            var folder = _context.Folder_Entities
                .AsNoTracking()
                .FirstOrDefault(f => f.ID == id);

            var childs = _context.Folder_Entities
                .AsNoTracking()
                .Where(c => c.OwnerID == id);

            folder.NestedFolders = new List<Folder_Entity>();

            if (childs != null)
                foreach (var item in childs)
                    folder.NestedFolders.Add(item);

            return View(folder);
        }

        // GET: Folders/Create
        public async Task<IActionResult> Create(int? id)
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

            folder_Entity.OwnerID = folder_Entity.ID;

            return View(folder_Entity);
        }

        // POST: Folders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,OwnerID")] Folder_Entity folder_Entity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(folder_Entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Folder));
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
                return RedirectToAction(nameof(Folder));
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
                return Problem("Directory is empty.");
            }
            var folder_Entity = await _context.Folder_Entities.FindAsync(id);
            if (folder_Entity != null)
            {
                _context.Folder_Entities.Remove(folder_Entity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Folder));
        }

        private bool Folder_EntityExists(int id)
        {
            return (_context.Folder_Entities?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        public IActionResult Import_Export()
        {
            FileModel fileModel = new FileModel();

            return View(fileModel);
        }

        [HttpPost]
        public async Task<IActionResult> Import_Export(FileModel fileModel)
        {
            if (fileModel == null || fileModel.file == null)
                return View();

            // Work with file
            string filePath = _webHostEnvironment.WebRootPath + "/UserFiles/Imports/" + Guid.NewGuid().ToString() + "_" + fileModel.file.FileName;

            using (var stream = System.IO.File.Create(filePath))
            {
                await fileModel.file.CopyToAsync(stream);
            }

            string UnsplittedLine = "";
            using (var stream = new StreamReader(filePath))
            {
                UnsplittedLine = stream.ReadToEnd();
            }

            // Splitting data into string array then splitting that strings to Folder_Entity items
            string[] UnsplittedData = UnsplittedLine.Split("\n");
            var Data = new Folder_Entity[UnsplittedData.Length];
            int i = 0;
            foreach (var item in UnsplittedData)
            {
                if (item != "" && item.Contains("_&_"))
                {
                    var SplittedData = new string[3];
                    SplittedData = item.Split("_&_");

                    // If data incorrect item will be skipped
                    if (SplittedData.Length != 3 ||
                        SplittedData[0] == null || SplittedData[1] == null || SplittedData[2] == null ||
                        SplittedData[0] == "" || SplittedData[1] == "" || SplittedData[2] == "" ||
                        !int.TryParse(SplittedData[0], out int result0) || !int.TryParse(SplittedData[2], out int result2))
                        continue;

                    Data[i] = new Folder_Entity { ID = result0, Name = SplittedData[1], OwnerID = result2 };

                    i++;
                }
            }

            if (Data.Length == 0)
                return View();

            // Remove all rows in table and creating new rows from Data array
            _context.Folder_Entities.RemoveRange(_context.Folder_Entities);
            foreach (Folder_Entity item in Data)
                if (item != null)
                    _context.Folder_Entities.Add(item);
            _context.SaveChanges();

            return RedirectToAction(nameof(Folder));
        }
    }
}
