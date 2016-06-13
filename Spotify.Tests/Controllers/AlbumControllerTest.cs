using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotify.Controllers;

namespace Spotify.Tests.Controllers
{
    [TestClass]
    public class AlbumControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            AlbumController controller = new AlbumController();
            ViewResult index = controller.Index(1) as ViewResult;
            Assert.IsNotNull(index);
        }
    }
}
