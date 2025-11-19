using Microsoft.AspNetCore.Mvc;
using TheHiddenSquirrel.Controllers;

namespace TheHiddenSquirrelTests
{
    [TestClass]
    public sealed class HomeControllerTests
    {
        [TestMethod]
        public void IndexReturnsView()
        {
            // check Index() method returns a view called "Index"
            // arrange: setup vars / objects needed
            var controller = new HomeController();

            // act: call method and get back the result.  Must cast IActionResult as ViewResult
            var result = (ViewResult)controller.Index();

            // assert: evaluate if result is what we expected
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void PrivacyReturnsView()
        {
            // arrange
            var controller = new HomeController();

            // act
            var result = (ViewResult)controller.Privacy();

            // assert
            Assert.AreEqual("Privacy", result.ViewName);
        }
    }
}
