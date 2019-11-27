using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System;

namespace Alexa_Functions.Tests
{
    [TestClass]
    public class RegisterTests : BaseTests
    {
        [TestMethod]
        public async Task Register_Valid_Test()
        {
            var handler = new Function(_MockSP.Object);
            var request = new APIGatewayProxyRequest()
            {
                HttpMethod = "POST",
                Body = JsonConvert.SerializeObject(new Models.Endpoint()
                {
                    UserId = "Test",
                    EndpointUrl = "google.com",
                    Token = "123456"
                })
            };


            var response = await handler.RegisterHandler(request, _MockContext.Object);

            Assert.AreEqual(200, response.StatusCode);
            _MockSP.Verify(f => f.GetService(typeof(IEndpointService)), Times.Once);
            _MockEndpoint.Verify(f => f.SaveEndpoint(It.IsAny<Models.Endpoint>()), Times.Once);
        }

        [TestMethod]
        public async Task Register_NoBody_Test()
        {
            var handler = new Function(_MockSP.Object);
            var request = new APIGatewayProxyRequest()
            {
                HttpMethod = "POST"
            };

            var response = await handler.RegisterHandler(request, _MockContext.Object);

            Assert.AreEqual(500, response.StatusCode);
            Assert.IsTrue(response.Body.Length > 0);
            _MockSP.Verify(f => f.GetService(typeof(IEndpointService)), Times.Never);
            _MockEndpoint.Verify(f => f.SaveEndpoint(It.IsAny<Models.Endpoint>()), Times.Never);
        }

        [TestMethod]
        public async Task Register_SaveFailed_Test()
        {
            _MockEndpoint.Setup(f => f.SaveEndpoint(It.IsAny<Models.Endpoint>())).ThrowsAsync(new Exception("Failed"));

            var handler = new Function(_MockSP.Object);
            var request = new APIGatewayProxyRequest()
            {
                HttpMethod = "POST",
                Body = JsonConvert.SerializeObject(new Models.Endpoint()
                {
                    UserId = "Test",
                    EndpointUrl = "google.com",
                    Token = "123456"
                })
            };

            var response = await handler.RegisterHandler(request, _MockContext.Object);

            Assert.AreEqual(500, response.StatusCode);
            Assert.IsTrue(response.Body.Contains("Failed"));
            _MockSP.Verify(f => f.GetService(typeof(IEndpointService)), Times.Once);
            _MockEndpoint.Verify(f => f.SaveEndpoint(It.IsAny<Models.Endpoint>()), Times.Once);
        }
    }
}
