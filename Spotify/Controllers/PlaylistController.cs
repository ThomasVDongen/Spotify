using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Spotify.Controllers
{
    public class PlaylistController : Controller
    {
        // GET: Playlist
        public ActionResult Playlist()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSong(int playlistid, int songid)
        {
            Database.AddSongToPlaylist(songid, playlistid);
            return RedirectToAction("Nummers", "Home");
        }
    }
}