using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;

namespace Alexa_Functions.Models
{
    public class Endpoint
    {
        public string UserId { get; set; }
        public string EndpointUrl { get; set; }
        public string Token { get; set; }

        public Endpoint() { }
        public Endpoint(Dictionary<string, AttributeValue> keys)
        {
            UserId = keys["userid"].S;
            EndpointUrl = keys["endpoint"].S;
            Token = keys["token"].S;
        }

        public Dictionary<string, AttributeValue> ConvertToDBDocument()
        {
            return new Dictionary<string, AttributeValue>()
            {
                { "userid", new AttributeValue(UserId) },
                { "endpoint", new AttributeValue(EndpointUrl) },
                { "token", new AttributeValue(Token) },
            };
        }
    }
}
