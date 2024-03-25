using FixIt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phone_spare_partes.data;

namespace FixIt.Controllers
{
    public class UsersDetailsController : Controller
    {
        private readonly PhoneSparePartsContext _context;

        public UsersDetailsController(PhoneSparePartsContext context)
        {
            _context = context;
        }

        // GET: UsersDetails
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'PhoneSparePartsContext.Users'  is null.");
        }


        // GET: UsersDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'PhoneSparePartsContext.users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: UsersDetails/Edit/5
        public async Task<IActionResult> Edit()
        {
            var name = HttpContext.Session.GetString("Email");
            var user = await _context.Users.FirstOrDefaultAsync(m => m.EMail == HttpContext.Session.GetString("Email"));

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: UsersDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UId,FName,LName,EMail,Pass,Address,Phone,clientFile")] Users users)
        {


            if (ModelState.IsValid)
            {
                if (users.ClientFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    users.ClientFile.CopyTo(stream);
                    users.Photo = stream.ToArray();
                }
                string input = users.Pass;
                if (!string.IsNullOrEmpty(input))
                {
                    users.Pass = pass_hash.Hashpassword(input);

                }
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Profile", "Home");
            }
            return View(users);
        }


        private bool UsersExists(int id)
        {
            return (_context.Users?.Any(e => e.UId == id)).GetValueOrDefault();
        }
    }
}
