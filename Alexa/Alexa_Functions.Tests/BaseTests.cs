using System;
using Moq;
using Home.Core.Clients.Interfaces;
using Amazon.Lambda.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alexa_Functions.Tests
{
    [TestClass]
    public class BaseTests
    {
        protected Mock<IServiceProvider> _MockSP;
        protected Mock<IEndpointService> _MockEndpoint;
        protected Mock<IGarage> _MockGarage;
        protected Mock<ILambdaContext> _MockContext;

        [TestInitialize]
        public void Init()
        {
            _MockContext = new Mock<ILambdaContext>();
            SetupMockEndpoint();
            SetupMockGarage();
            SetupServiceProvider();
        }

        private void SetupServiceProvider()
        {
            _MockSP = new Mock<IServiceProvider>();
            _MockSP.Setup(f => f.GetService(typeof(IGarage))).Returns(_MockGarage.Object);
            _MockSP.Setup(f => f.GetService(typeof(IEndpointService))).Returns(_MockEndpoint.Object);
        }

        private void SetupMockGarage()
        {
            _MockGarage = new Mock<IGarage>();
            _MockGarage.Setup(f => f.GetGarageStatus()).ReturnsAsync(0);
        }

        private void SetupMockEndpoint()
        {
            _MockEndpoint = new Mock<IEndpointService>();
            _MockEndpoint.Setup(f => f.GetEndpoint(It.IsAny<string>())).ReturnsAsync(new Models.Endpoint() { UserId = "test", EndpointUrl = "google.com", Token = "t1" });
            _MockEndpoint.Setup(f => f.SaveEndpoint(It.IsAny<Models.Endpoint>())).ReturnsAsync(true);
        }
    }
}
