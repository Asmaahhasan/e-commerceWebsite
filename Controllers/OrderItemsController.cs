using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FixIt.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly PhoneSparePartsContext _context;

        public OrderItemsController(PhoneSparePartsContext context)
        {
            _context = context;
        }

        // GET: OrderItems
        public async Task<IActionResult> Index()
        {
            var phoneSparePartsContext = _context.OrderItems.Include(o => o.OidNavigation).Include(o => o.SIdNavigation);
            return View(await phoneSparePartsContext.ToListAsync());
        }

        public async Task<IActionResult> ItemsForOrder(int OrderId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.EMail == HttpContext.Session.GetString("Email"));


            IEnumerable<OrderItem> model = _context.OrderItems.Where(p => p.Oid == OrderId).Include(s => s.SIdNavigation).Include(o => o.OidNavigation).Include(s => s.SIdNavigation.CIdNavigation);
            return View(model);
        }
        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.OidNavigation)
                .Include(o => o.SIdNavigation)
                .FirstOrDefaultAsync(m => m.OitemId == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: OrderItems/Create
        public IActionResult Create()
        {
            ViewData["Oid"] = new SelectList(_context.Payments, "Oid", "Oid");
            ViewData["SId"] = new SelectList(_context.SpareParts, "SId", "SId");
            return View();
        }

        // POST: OrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OitemId,Oid,SId,Quantity,UnitPrice")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Oid"] = new SelectList(_context.Payments, "Oid", "Oid", orderItem.Oid);
            ViewData["SId"] = new SelectList(_context.SpareParts, "SId", "SId", orderItem.SId);
            return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewData["Oid"] = new SelectList(_context.Payments, "Oid", "Oid", orderItem.Oid);
            ViewData["SId"] = new SelectList(_context.SpareParts, "SId", "SId", orderItem.SId);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OitemId,Oid,SId,Quantity,UnitPrice")] OrderItem orderItem)
        {
            if (id != orderItem.OitemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.OitemId))
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
            ViewData["Oid"] = new SelectList(_context.Payments, "Oid", "Oid", orderItem.Oid);
            ViewData["SId"] = new SelectList(_context.SpareParts, "SId", "SId", orderItem.SId);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .Include(o => o.OidNavigation)
                .Include(o => o.SIdNavigation)
                .FirstOrDefaultAsync(m => m.OitemId == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.OitemId == id);
        }
    }
}
