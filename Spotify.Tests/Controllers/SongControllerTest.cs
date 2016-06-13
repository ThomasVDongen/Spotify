using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotify.Controllers;

namespace Spotify.Tests.Controllers
{
    [TestClass]
    public class SongControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            SongController controller = new SongController();
            ViewResult index = controller.Song(1) as ViewResult;
            ViewResult Add = controller.Add(1) as ViewResult;
            Assert.IsNotNull(Add);
            Assert.IsNotNull(index);

        }
    }
}
