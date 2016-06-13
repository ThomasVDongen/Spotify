using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotify.Controllers;
using Spotify.Models.Objecten;

namespace Spotify.Tests.Controllers
{
    [TestClass]
    public class LoginControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            LoginController controller = new LoginController();
            Account account = new Account();
            account.ID = 1;
            account.Email = "thomasvandongen019@hotmail.com";
            account.Password = "wachtwoord";
            ViewResult index = controller.Index() as ViewResult;
            ViewResult login = controller.Index(account) as ViewResult;
            ViewResult logout = controller.Logout() as ViewResult;
            Assert.IsNotNull(login);
            Assert.IsNotNull(logout);
            Assert.IsNotNull(index);
            string commit;

        }
    }
}
