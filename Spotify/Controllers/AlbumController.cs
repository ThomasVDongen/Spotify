using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Spotify.Controllers
{
    public class AlbumController : Controller
    {
        // GET: Album
        [HttpGet]
        public ActionResult Index(int id)
        {
            return View(Database.GetAlbum(id));
        }
    }
}