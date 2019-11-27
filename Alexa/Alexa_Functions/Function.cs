using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa_Functions.Models;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Home.Core.Clients.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Alexa_Functions
{
    public class Function
    {
        private readonly IServiceProvider _ServiceProvider;

        public Function() : this(StartUp.Container.BuildServiceProvider())
        {
        }

        public Function(IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider;
        }

        public async Task<APIGatewayProxyResponse> RegisterHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var resp = new APIGatewayProxyResponse() { StatusCode = 200 };
            try
            {
                var endpoint = JsonConvert.DeserializeObject<Endpoint>(request.Body);
                IEndpointService endpointService = _ServiceProvider.GetService<IEndpointService>();
                await endpointService.SaveEndpoint(endpoint);
            }
            catch(Exception ex)
            {
                resp.StatusCode = 500;
                resp.Body = ex.ToString();
            }

            return resp;
        }

        public async Task<SkillResponse> AlexaHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = null;
            var t = input.GetRequestType();

            if(t == typeof(IntentRequest))
            {
                response = await HandleIntentRequest(input);
            }
            else if(t == typeof(LaunchRequest))
            {
                // user wants to open the garage. ask for a confirmation.
                response = HandleLaunchRequest(input);
            }
            else
            {
                response = CreateBasicSkillResponse();
            }
            
            return response;
        }

        private async Task<SkillResponse> HandleIntentRequest(SkillRequest input)
        {
            IntentRequest request = input.Request as IntentRequest;
            var response = CreateBasicSkillResponse();

            // get endpoint values
            IEndpointService endpointService = _ServiceProvider.GetService<IEndpointService>();
            var endpoint = await endpointService.GetEndpoint(input.Context.System.User.UserId);

            if (endpoint == null)
            {
                response.Response.OutputSpeech = new PlainTextOutputSpeech("Endpoint is not setup");
                return response;
            }

            // get garage service
            var garageService = GetGarageService(endpoint);
            string result = null;

            switch (request.Intent.Name)
            {
                case "GetStatus":
                    result = await GetStatus(garageService, request.Intent);
                    break;

                case "Move":
                    result = await MoveGarage(garageService, request.Intent);
                    break;
            }

            if(result != null) response.Response.OutputSpeech = new PlainTextOutputSpeech(result);

            return response;
        }

        private SkillResponse HandleLaunchRequest(SkillRequest input)
        {
            var response = CreateBasicSkillResponse();
            response.Response.OutputSpeech = new PlainTextOutputSpeech("Invalid request. try saying ask garage door to open");

            return response;
        }

        private SkillResponse CreateBasicSkillResponse()
        {
            var response = new SkillResponse()
            {
                Version = "1.0",
                Response = new ResponseBody()
                {
                    ShouldEndSession = true,
                    OutputSpeech = new PlainTextOutputSpeech("Invalid Request")
                }
            };
            return response;
        }

        private async Task<string> GetStatus(IGarage garage, Intent intent)
        {
            string askedStatus = null;
            if (intent.Slots.ContainsKey("status")) askedStatus = intent.Slots["status"].Resolution?.Authorities.FirstOrDefault()?.Values.FirstOrDefault()?.Value.Id;

            int value = await garage.GetGarageStatus();

            var prefix = askedStatus == null ? "" : (askedStatus == value.ToString() ? "Yes, " : "No, ");

            return $"{prefix}the garage is {ConvertStatus(value)}";
        }

        private async Task<string> MoveGarage(IGarage garage, Intent intent)
        {
            var action = intent.Slots["action"].Resolution?.Authorities.FirstOrDefault()?.Values.FirstOrDefault()?.Value.Id;
            var currentStatus = await garage.GetGarageStatus();
            if (currentStatus.ToString() == action) return $"Garage is already {ConvertStatus(currentStatus)}";
            await garage.ToggleGarage();
            return $"Garage is {action}ing";
        }

        private IGarage GetGarageService(Endpoint endpoint)
        {
            var client = _ServiceProvider.GetService<IGarage>();
            client.UpdateBaseAndToken(endpoint.EndpointUrl, endpoint.Token);
            return client;
        }

        private string ConvertStatus(int status)
        {
            var val = "open";
            if (status == 0) val = "closed";
            return val;
        }
    }
}
