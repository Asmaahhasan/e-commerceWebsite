using FixIt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FixIt.Controllers
{

    public class AdminSpareController : Controller
    {
        readonly PhoneSparePartsContext db = new PhoneSparePartsContext();
        public async Task<IActionResult> Index()
        {
            var phoneSparePartsContext = db.SpareParts.Include(s => s.CIdNavigation);

            return View(await phoneSparePartsContext.ToListAsync());
        }

        // GET: SpareParts/Details/5

        public async Task<IActionResult> Details_Spare(int? id)
        {
            if (id == null || db.SpareParts == null)
            {
                return NotFound();
            }

            var sparePart = await db.SpareParts
                .Include(s => s.CIdNavigation)
                .FirstOrDefaultAsync(m => m.SId == id);
            if (sparePart == null)
            {
                return NotFound();
            }

            return View(sparePart);
        }



        public IActionResult Add_Spare()
        {
            ViewData["CId"] = new SelectList(db.Catagories, "CId", "CName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add_SpareAsync([Bind("SId,SName,Description,Price,CId,clientFile")] SparePart sparePart)
        {
            var cat = db.Catagories.Where(s => s.CId == sparePart.CId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (sparePart.ClientFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    sparePart.ClientFile.CopyTo(stream);
                    sparePart.Photo = stream.ToArray();
                }
                db.Add(sparePart);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CId"] = new SelectList(db.Catagories, "CId", "CName", cat.CName);
            return View(sparePart);

        }

        private bool SparePartExists(int id)
        {
            return (db.SpareParts?.Any(e => e.SId == id)).GetValueOrDefault();
        }




        public async Task<IActionResult> Edit_Spare(int? id)
        {

            if (id == null || db.SpareParts == null)
            {
                return NotFound();
            }

            var sparePart = await db.SpareParts.FindAsync(id);
            var cat = db.Catagories.Where(s => s.CId == sparePart.CId).FirstOrDefault();

            if (sparePart == null)
            {
                return NotFound();
            }
            ViewData["CId"] = new SelectList(db.Catagories, "CId", "CName", cat);
            return View(sparePart);
        }

        // POST: SpareParts/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_Spare(int id, [Bind("SId,SName,Description,Price,CId,clientFile")] SparePart sparePart)
        {

            var cat = db.Catagories.Where(s => s.CId == sparePart.CId).FirstOrDefault();
            if (sparePart.ClientFile != null)
            {
                MemoryStream stream = new MemoryStream();
                sparePart.ClientFile.CopyTo(stream);
                sparePart.Photo = stream.ToArray();
            }

            if (id != sparePart.SId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(sparePart);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SparePartExists(sparePart.SId))
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
            ViewData["CId"] = new SelectList(db.Catagories, "CId", "CName", cat);
            return View(sparePart);
        }
        public async Task<IActionResult> Delete_Spare(int? id)
        {
            if (id == null || db.SpareParts == null)
            {
                return NotFound();
            }

            var sparePart = await db.SpareParts
                .Include(s => s.CIdNavigation)
                .FirstOrDefaultAsync(m => m.SId == id);
            if (sparePart == null)
            {
                return NotFound();
            }

            return View(sparePart);
        }

        // POST: SpareParts/Delete/5
        [HttpPost, ActionName("Delete_Spare")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete_SpareConfirmed(int id)
        {
            if (db.SpareParts == null)
            {
                return Problem("Entity set 'PhoneSparePartsContext.SpareParts'  is null.");
            }
            var sparePart = await db.SpareParts.FindAsync(id);
            if (sparePart != null)
            {
                db.SpareParts.Remove(sparePart);
            }
            //db.Remove(sparePart);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}


