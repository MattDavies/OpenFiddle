using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenFiddle.Controllers;
using OpenFiddle.Models.Console;

namespace OpenFiddle.Tests.Controllers
{
    [TestClass]
    public class ConsoleControllerTest
    {
        [TestMethod]
        public void RunHelloWorld()
        {
            // Arrange
            ConsoleController controller = new ConsoleController();

            string helloWorldCode = OpenFiddle.Resources.CodeSamples.HelloWorldConsole;

            ConsoleInput input= new ConsoleInput
            {
                Id = Guid.NewGuid(),
                Code = helloWorldCode
            };

            // Act
            var result = controller.Run(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(input.Id, result.Id);
            Assert.AreEqual(helloWorldCode, result.Code);
            Assert.AreEqual("Hello World\r\n", result.Output);
        }
    }
}
