using FixIt.Models;
using Microsoft.AspNetCore.Mvc;
using phone_spare_partes.data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace FixIt.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor? _context;
        [Obsolete]
        private readonly IHostingEnvironment? _host;
        readonly PhoneSparePartsContext db = new();
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Users _user)
        {
            if (ModelState.IsValid)
            {

                if (_user.ClientFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    _user.ClientFile.CopyTo(stream);
                    _user.Photo = stream.ToArray();
                }
                var check = db.Users.FirstOrDefault(s => s.EMail == _user.EMail);
                if (check == null)
                {

                    string input = _user.Pass;
                    if (!string.IsNullOrEmpty(input))
                    {
                        _user.Pass = pass_hash.Hashpassword(input);

                    }
                    db.Users.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }

            }
            return View();

        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users us, string returnUrl)
        {
            var obj = db.Users.Where(a => a.EMail.Equals(us.EMail) && a.Pass.Equals(pass_hash.Hashpassword(us.Pass))).FirstOrDefault();
            if (obj == null)
            {
                ViewBag.ErrorMessage = "Email or PassWord Is Not Correct";
                return View();
            }

            else
            {

                if (us.EMail.ToLower().Equals("admina@gmail.com"))
                {

                    var sessionId = HttpContext.Session.Id;
                    HttpContext.Session.SetString("Email", us.EMail);
                    Response.Cookies.Append("SessionId", sessionId, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    });
                    
                    return RedirectToAction("Index", "AdminSpare");
                }
                else
                {
                    var sessionId = HttpContext.Session.Id;
                    HttpContext.Session.SetString("Email", us.EMail);
                    Response.Cookies.Append("SessionId", sessionId, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    });
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                        return RedirectToAction("Home", "Home");

                }
            }
        }
        public ActionResult Logout()
        {
            Response.Cookies.Delete("SessionId");
            HttpContext.Session.Clear();
            return RedirectToAction("Home", "Home");
        }

    }
}
