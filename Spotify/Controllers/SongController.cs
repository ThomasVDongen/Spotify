using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spotify.Models;
using Spotify.Models.Objecten;

namespace Spotify.Controllers
{
    public class SongController : Controller
    {
        // GET: Song
        [ChildActionOnly]
        public ActionResult Song(int id)
        {
            return PartialView(Database.GetSong(id));
        }

        [HttpGet]
        public ActionResult Add(int id)
        {
            if (check())
            {
                AccountSongID accountS = new AccountSongID();
                accountS.Account = Database.GetAccount(User.Identity.Name);
                accountS.ID = id;
                return PartialView(accountS);
            }
            return RedirectToAction("Index", "Login");
        }

        public bool check()
        {
            if (User.Identity.Name != null)
            {
                return true;
            }
            return false;
        }
    }
}