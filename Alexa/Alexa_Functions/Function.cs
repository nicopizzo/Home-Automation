using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Request.Type;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Alexa_Functions
{
    public class Function
    {
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {

        }


        /// <summary>
        /// This method is called for every Lambda invocation.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            var response = new SkillResponse()
            {
                Version = "1.0", Response = new ResponseBody()
                {
                    ShouldEndSession = true,
                    OutputSpeech = new PlainTextOutputSpeech("Invalid Request")
                } 
            };
            var t = input.GetRequestType();

            if(t == typeof(IntentRequest))
            {
                IntentRequest request = input.Request as IntentRequest;
                string result = null;
                switch (request.Intent.Name)
                {
                    case "GetStatus":
                        result = GetStatus();
                        response.Response.OutputSpeech = new PlainTextOutputSpeech($"Garage Door is {result}");
                        break;

                    case "Move":
                        result = MoveGarage("d");
                        response.Response.OutputSpeech = new PlainTextOutputSpeech(result);
                        break;
                }
            }
            
            return response;
        }

        private string GetStatus()
        {
            return "Open";
        }

        private string MoveGarage(string action)
        {
            return "moving";
        }
    }
}
