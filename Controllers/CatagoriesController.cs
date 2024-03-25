using FixIt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FixIt.Controllers
{
    public class CatagoriesController : Controller
    {
        private readonly PhoneSparePartsContext _context;

        public CatagoriesController(PhoneSparePartsContext context)
        {
            _context = context;
        }

        // GET: Catagories
        public async Task<IActionResult> Index()
        {
            var phoneSparePartsContext = _context.Catagories.Include(c => c.BIdNavigation);
            return View(await phoneSparePartsContext.ToListAsync());
        }
        public IActionResult CategoriesByBrand(int brandId, string Search)
        {
            Catagory rec = new Catagory
            {
                BId = brandId
            };
            ViewData["Message"] = rec;
            IEnumerable<Catagory> model = _context.Catagories.Where(p => p.BId == brandId).Include(s => s.BIdNavigation);
            if (!model.Any())
            {
                return NotFound($"No products  {brandId:C}.");
            }
            if (!String.IsNullOrEmpty(Search))
            {
                var search = model.Where(n => n.CName.Contains(Search, StringComparison.CurrentCultureIgnoreCase)).ToList();
                return View(search);
            }
            else
            {
                return View(model);
            }
        }
        // GET: Catagories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Catagories == null)
            {
                return NotFound();
            }

            var catagory = await _context.Catagories
                .Include(c => c.BIdNavigation)
                .FirstOrDefaultAsync(m => m.CId == id);
            if (catagory == null)
            {
                return NotFound();
            }

            return View(catagory);
        }

        // GET: Catagories/Create
        public IActionResult Create()
        {
            ViewData["BId"] = new SelectList(_context.Brands, "BId", "BName");
            return View();
        }

        // POST: Catagories/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CId,CName,BId,clientFile")] Catagory catagory)
        {

            if (ModelState.IsValid)
            {
                if (catagory.clientFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    catagory.clientFile.CopyTo(stream);
                    catagory.Photo = stream.ToArray();
                }
                _context.Add(catagory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BId"] = new SelectList(_context.Brands, "BId", "BId", catagory.BId);
            return View(catagory);
        }

        // GET: Catagories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Catagories == null)
            {
                return NotFound();
            }
            var category = await _context.Catagories.FindAsync(id);
            var b = _context.Brands.Where(s => s.BId == category.CId).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }
            ViewData["BId"] = new SelectList(_context.Brands, "BId", "BName", b);
            return View(category);
        }

        // POST: Catagories/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CId,CName,BId,clientFile")] Catagory catagory)
        {
            if (id != catagory.CId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (catagory.clientFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    catagory.clientFile.CopyTo(stream);
                    catagory.Photo = stream.ToArray();
                }
                try
                {
                    _context.Update(catagory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatagoryExists(catagory.CId))
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
            ViewData["BId"] = new SelectList(_context.Brands, "BId", "BId", catagory.BId);
            return View(catagory);
        }

        // GET: Catagories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Catagories == null)
            {
                return NotFound();
            }

            var catagory = await _context.Catagories
                .Include(c => c.BIdNavigation)
                .FirstOrDefaultAsync(m => m.CId == id);
            if (catagory == null)
            {
                return NotFound();
            }

            return View(catagory);
        }

        // POST: Catagories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Catagories == null)
            {
                return Problem("Entity set 'PhoneSparePartsContext.Catagories'  is null.");
            }
            foreach (var spare in _context.SpareParts)
            {
                if (spare.CId == id)
                {

                    _context.SpareParts.Remove(spare);

                }
            }
            var catagory = await _context.Catagories.FindAsync(id);
            if (catagory != null)
            {
                _context.Catagories.Remove(catagory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatagoryExists(int id)
        {
            return (_context.Catagories?.Any(e => e.CId == id)).GetValueOrDefault();
        }
    }
}
