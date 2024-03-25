using FixIt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
namespace FixIt.Controllers
{
    public class CartItemsController : Controller
    {
        PhoneSparePartsContext _context = new PhoneSparePartsContext();


        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            var phoneSparePartsContext = _context.Carts.Include(c => c.SIdNavigation).Include(c => c.UIdNavigation);
            return View(await phoneSparePartsContext.ToListAsync());
        }

        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.Carts
                .Include(c => c.SIdNavigation)
                .Include(c => c.UIdNavigation)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // GET: CartItems/Create
        public async Task<IActionResult> Create(int partId)
        {
            Response.Headers.Append("Cache-Control", "no-cache,no-store,must-revalidate");
            Response.Headers.Append("Pragma", "no-cache");

            var name = HttpContext.Session.GetString("Email");
            if (String.IsNullOrEmpty(name))
            {

                return RedirectToAction("Login", "user");
            }
            var user = await _context.Users.FirstOrDefaultAsync(m => m.EMail == HttpContext.Session.GetString("Email"));
            var model = _context.Carts.Where(p => p.UId == user.UId);
            foreach (var item in model)
            {
                if (partId == item.SId)
                {
                    RedirectToAction("Edit", item.CartId);

                }
            }
            var sparePart = await _context.SpareParts.FirstOrDefaultAsync(m => m.SId == partId);
            var cat = await _context.Catagories.FirstOrDefaultAsync(m => m.CId == sparePart.CId);
            sparePart.CIdNavigation = cat;

            var viewModel = new Cart
            {
                SIdNavigation = sparePart,
                UIdNavigation = user,

                SId = partId,

                UId = user.UId,
                Quantity = 1

            };

            return View(viewModel);
        }

        // POST: CartItems/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,SId,UId,Quantity")] Cart cartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("UserCart");
            }
            ViewData["SId"] = new SelectList(_context.SpareParts, "SId", "SId", cartItem.SId);
            ViewData["UId"] = new SelectList(_context.Users, "UId", "Address", cartItem.UId);
            return RedirectToAction("brands", "brands");
        }

        public async Task<IActionResult> UserCart()
        {
            Response.Headers.Append("Cache-Control", "no-cache,no-store,must-revalidate");
            Response.Headers.Append("Pragma", "no-cache");

            var name = HttpContext.Session.GetString("Email");
            if (String.IsNullOrEmpty(name))
            {
                var returnUrl = Request.Path.Value;
                return RedirectToAction("Login", "user", new { returnUrl });
            }
            var user = await _context.Users.FirstOrDefaultAsync(m => m.EMail == HttpContext.Session.GetString("Email"));
            var model = _context.Carts.Where(p => p.UId == user.UId).Include(o => o.SIdNavigation).Include(o => o.UIdNavigation).Include(o => o.SIdNavigation.CIdNavigation);
            return View(await model.ToListAsync());


        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.Carts.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            ViewData["SId"] = new SelectList(_context.SpareParts, "SId", "SId", cartItem.SId);
            ViewData["UId"] = new SelectList(_context.Users, "UId", "Address", cartItem.UId);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,SId,UId,Quantity")] Cart cartItem)
        {
            if (id != cartItem.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.CartId))
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
            ViewData["SId"] = new SelectList(_context.SpareParts, "SId", "SId", cartItem.SId);
            ViewData["UId"] = new SelectList(_context.Users, "UId", "Address", cartItem.UId);
            return View(cartItem);
        }

        //public IActionResult UpdateCartItemQuantity(int itemId, int newQuantity)
        //{
        //    using (var context = new  PhoneSparePartsContext())
        //    {
        //        var cartItem = context.Carts.Find(itemId);
        //        if (cartItem != null)
        //        {
        //            cartItem.Quantity = newQuantity;
        //            context.SaveChanges();

        //            return Ok(); // Return success response
        //        }
        //        else
        //        {
        //            return NotFound(); // Handle case where cart item not found
        //        }
        //    }
        //}

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var cartItem = await _context.Carts.FindAsync(id);
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("UserCart");
        }


        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.EMail == HttpContext.Session.GetString("Email"));
            Payment payment = new()
            {
                UserId = user.UId,
                OrderDate = DateTime.Now,
                TotalAmount = ((decimal?)_context.Carts.Where(c => c.UId == user.UId).Sum(c => c.SIdNavigation.Price * c.Quantity))
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            foreach (var cartItem in _context.Carts.Include(payment => payment.SIdNavigation).Where(ci => ci.UId == user.UId))
            {



                var order = new OrderItem
                {
                    OidNavigation = payment,
                    Oid = payment.Oid,
                    SId = cartItem.SId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = (decimal)(cartItem.SIdNavigation.Price * cartItem.Quantity)
                };






                _context.OrderItems.Add(order);


            }
            _context.Carts.RemoveRange(_context.Carts.Where(ci => ci.UId == user.UId));
            _context.SaveChanges();


            return RedirectToAction("usercart");
        }

        private bool CartItemExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }
    }
}
