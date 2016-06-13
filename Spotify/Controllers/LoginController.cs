using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Spotify.Models.Objecten;

namespace Spotify.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpPost]
        public ActionResult Index(Account account)
        {
            if (ModelState.IsValid)
            {
                if (account.LoginCorrect(account.Email, account.Password))
                {
                    FormsAuthentication.SetAuthCookie(account.Email,true);
                    return RedirectToAction("Index", "Home");
                }
                else ModelState.AddModelError("", "Login credentials were incorrect!");
            }
            return View(account);
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            else return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}