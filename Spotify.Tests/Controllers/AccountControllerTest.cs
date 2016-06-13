using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotify.Controllers;

namespace Spotify.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void Index()
        {
            AccountController controller = new AccountController();

            ViewResult index = controller.Index() as ViewResult;

            Assert.IsNotNull(index);
        }
    }
}
