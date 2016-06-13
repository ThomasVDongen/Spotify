using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotify.Controllers;

namespace Spotify.Tests.Controllers
{
    [TestClass]
    public class MusicControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            MusicController controller = new MusicController();
            ViewResult zoek = controller.Zoek("purpose") as ViewResult;
            ViewResult removesong = controller.SongRemovePlaylist(5, 3) as ViewResult;
            Assert.IsNotNull(zoek);
            Assert.IsNotNull(removesong);

        }
    }
}
