using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenFiddle.Models.Ide;
using OpenFiddle.Controllers;

namespace Openfiddle.Tests.Controllers
{
    [TestClass]
    public class IDEControllerTest
    {
        [TestMethod]
        public void AutoCompleteTest()
        {
            //TODO
        }

        [TestMethod]
        public void FormatTest()
        {
            // Arrange
            IDEController controller = new IDEController();

            string helloWorldCode = OpenFiddle.Resources.CodeSamples.HelloWorldConsoleCSharp;

            var modifiedCode = helloWorldCode.Insert(20, "\t\t");

            CodeInput input = new CodeInput
            {
                Code = modifiedCode,
                Language = OpenFiddle.Models.Shared.Language.CSharp
            };

            // Act
            var result = controller.Format(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(helloWorldCode, result);
        }

        [TestMethod]
        public void CheckSyntaxTest()
        {
        }

        [TestMethod]
        public void ConvertTest()
        {
        }

        [TestMethod]
        public void GuidTest()
        {
        }
    }
}
