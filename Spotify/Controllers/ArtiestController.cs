using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Spotify.Controllers
{
    public class ArtiestController : Controller
    {
        // GET: Artiest
        [HttpGet]
        public ActionResult Index(int artistid)
        {
            return View(Database.GetArtist(artistid));
        }
    }
}