using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotify.Controllers;

namespace Spotify.Tests.Controllers
{
    [TestClass]
    public class PlaylistControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            PlaylistController controller = new PlaylistController();
            ViewResult addsong = controller.AddSong(5,1) as ViewResult;
            Assert.IsNotNull(addsong);
        }
    }
}
