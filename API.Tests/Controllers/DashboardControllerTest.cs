using System.Net.Http;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LifeSpan.DTO.Common;
using LifeSpan.API;
using API.Controllers;
using LifeSpan.DTO;

namespace API.Tests.Controllers
{
    [TestClass]
    public class DashboardControllerTest
    {
        [TestMethod]
        public void StaffBirthdays()
        {
            // Arrange
            DashboardController controller = new DashboardController();

            // Act
            HttpResponseMessage result = controller.StaffBirthdays(LifeSpan.DTO.Common.) as HttpResponseMessage;

            // Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
