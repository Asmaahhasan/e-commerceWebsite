using FixIt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FixIt.Controllers
{
    public class SparePartsController : Controller
    {
        private readonly PhoneSparePartsContext _context;

        public SparePartsController(PhoneSparePartsContext context)
        {
            _context = context;
        }

        // GET: SpareParts
        public async Task<IActionResult> Index()
        {
            var phoneSparePartsContext = _context.SpareParts.Include(s => s.CIdNavigation);
            return View(await phoneSparePartsContext.ToListAsync());
        }
        public IActionResult Sparepartsbycatagory(int catId, string Search)
        {
            SparePart recc = new SparePart
            {
                CId = catId
            };
            ViewData["ss"] = recc;

            IEnumerable<SparePart> model = _context.SpareParts.Where(p => p.CId == catId).Include(s => s.CIdNavigation);
            if (!model.Any())
            {
                return NotFound($"No products  {catId:C}.");
            }
            if (!String.IsNullOrEmpty(Search))
            {
                var search = model.Where(n => n.SName.ToLower().Contains(Search.ToLower())).ToList();
                return View(search);
            }
            else
            {
                return View(model);
            }
        }

        // GET: SpareParts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SpareParts == null)
            {
                return NotFound();
            }

            var sparePart = await _context.SpareParts
                .Include(s => s.CIdNavigation)
                .FirstOrDefaultAsync(m => m.SId == id);
            if (sparePart == null)
            {
                return NotFound();
            }

            return View(sparePart);
        }

        // GET: SpareParts/Create
        public IActionResult Create()
        {
            ViewData["CId"] = new SelectList(_context.Catagories, "CId", "CId");
            return View();
        }

        // POST: SpareParts/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SId,SName,Describtion,Price,CId,Photo")] SparePart sparePart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sparePart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CId"] = new SelectList(_context.Catagories, "CId", "CId", sparePart.CId);
            return View(sparePart);
        }

        // GET: SpareParts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SpareParts == null)
            {
                return NotFound();
            }

            var sparePart = await _context.SpareParts.FindAsync(id);
            if (sparePart == null)
            {
                return NotFound();
            }
            ViewData["CId"] = new SelectList(_context.Catagories, "CId", "CId", sparePart.CId);
            return View(sparePart);
        }

        // POST: SpareParts/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SId,SName,Describtion,Price,CId,Photo")] SparePart sparePart)
        {
            if (id != sparePart.SId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sparePart);
                    await _context.SaveChangesAsync();
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
            ViewData["CId"] = new SelectList(_context.Catagories, "CId", "CId", sparePart.CId);
            return View(sparePart);
        }

        // GET: SpareParts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SpareParts == null)
            {
                return NotFound();
            }

            var sparePart = await _context.SpareParts
                .Include(s => s.CIdNavigation)
                .FirstOrDefaultAsync(m => m.SId == id);
            if (sparePart == null)
            {
                return NotFound();
            }

            return View(sparePart);
        }

        // POST: SpareParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SpareParts == null)
            {
                return Problem("Entity set 'PhoneSparePartsContext.SpareParts'  is null.");
            }
            var sparePart = await _context.SpareParts.FindAsync(id);
            if (sparePart != null)
            {
                _context.SpareParts.Remove(sparePart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SparePartExists(int id)
        {
            return (_context.SpareParts?.Any(e => e.SId == id)).GetValueOrDefault();
        }
    }
}
