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
        public ActionResult Index()
        {
            return View();
        }

       public ActionResult Nummers()
        {
            return View();
        }

        public ActionResult Artiesten()
        {
            return View();
        }

        public ActionResult Albums()
        {
            return View();
        }

        public ActionResult Playlists(Playlist playlist)
        {
            return View(playlist);
        }

    }
}