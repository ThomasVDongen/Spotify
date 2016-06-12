using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spotify.Models.Objecten;

namespace Spotify.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(Database.GetAccount(User.Identity.Name));
        }
        [HttpGet]
        public ActionResult Nummers()
        {
            Account account = Database.GetAccount(User.Identity.Name);
            return View(Database.GetSongs(account.ID));
        }

        public ActionResult Artiesten()
        {
            Account account = Database.GetAccount(User.Identity.Name);
            return View(Database.GetArtists(account.ID));
        }

        public ActionResult Albums()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Playlist(int id)
        {
            return View(Database.GetPlaylist(id));
        }
        [ChildActionOnly]
        public ActionResult PlaylistsPartial()
        {
            return PartialView(Database.GetAccount(User.Identity.Name));
        }

        [ChildActionOnly]
        public ActionResult AccountPartial()
        {
            return PartialView(Database.GetAccount(User.Identity.Name));
        }

    }
}