using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spotify.Models.Objecten;

namespace Spotify.Controllers
{
    public class MusicController : Controller
    {
        // GET: Music
        [HttpPost]
        public ActionResult SongRemovePlaylist(int playlistid, int songid)
        {
            Account account = Database.GetAccount(User.Identity.Name);
            Database.SongRemovePlaylist(playlistid, songid);
            return RedirectToAction("Playlist", "Home", new {id = playlistid});
        }

        public ActionResult Zoek(string zoekwaarde)
        {
           return View(Database.Zoeken(zoekwaarde));
        }
    }
}