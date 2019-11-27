using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alexa_Functions.Tests
{
    [TestClass]
    public class AlexaTests : BaseTests
    {
        [TestMethod]
        public async Task InvalidRequest_Test()
        {
            var request = new SkillRequest();

            var handler = new Function(_MockSP.Object);

            var response = await handler.AlexaHandler(request, _MockContext.Object);
            Assert.IsNotNull(response);
            Assert.AreEqual("1.0", response.Version);
            Assert.IsTrue(response.Response.ShouldEndSession.Value);
            Assert.AreEqual("Invalid Request", ((PlainTextOutputSpeech)response.Response.OutputSpeech).Text);
        }
    }
}
