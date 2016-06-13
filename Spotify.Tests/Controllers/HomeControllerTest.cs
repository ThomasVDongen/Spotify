using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotify;
using Spotify.Controllers;

namespace Spotify.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();
            // Act
            ViewResult index = controller.Index() as ViewResult;
            ViewResult accountPartial = controller.AccountPartial() as ViewResult;
            ViewResult playlist = controller.Playlist(1) as ViewResult;
            ViewResult nummers = controller.Nummers() as ViewResult;
            ViewResult albums = controller.Albums() as ViewResult;
            ViewResult artiesten = controller.Artiesten() as ViewResult;
            ViewResult playlistPartial = controller.PlaylistsPartial() as ViewResult;
            // Assert
            Assert.IsNotNull(index);
            Assert.IsNotNull(accountPartial);
            Assert.IsNotNull(playlist);
            Assert.IsNotNull(nummers);
            Assert.IsNotNull(albums);
            Assert.IsNotNull(artiesten);
            Assert.IsNotNull(playlistPartial);
        }

        

      }
}
