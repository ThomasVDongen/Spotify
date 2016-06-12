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

        [HttpPost]
        public ActionResult Add(int id)
        {
            AccountSongID accountS = new AccountSongID();
            accountS.Account = Database.GetAccount(User.Identity.Name);
            accountS.ID = id;
            return PartialView(accountS);
        }
    }
}