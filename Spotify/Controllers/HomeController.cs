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
           
            return View(Database.getAccount(User.Identity.Name));
        }

       public ActionResult Nummers()
        {
            return View();
        }

        public ActionResult Artiesten(Artist artist)
        {
            return View();
        }

        public ActionResult Albums(Album album)
        {
            return View();
        }
        
        public ActionResult Playlist(int playlist)
        {
            
            return View(playlist);
        }

    }
}