using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa_Functions.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;

namespace Alexa_Functions
{
    public class EndpointService : IEndpointService
    {
        private readonly IAmazonDynamoDB _Client;
        private readonly IOptions<DBSettings> _DBSettings;

        public EndpointService(IAmazonDynamoDB client, IOptions<DBSettings> dbSettings)
        {
            _Client = client;
            _DBSettings = dbSettings;
        }

        public async Task<Models.Endpoint> GetEndpoint(string userId)
        {
            var request = new GetItemRequest(_DBSettings.Value.TableName, new Dictionary<string, AttributeValue>() { { "userid", new AttributeValue(userId) } });
            var response = await _Client.GetItemAsync(request);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK) return null;
            var e = new Models.Endpoint(response.Item);
            return e;
        }

        public async Task<bool> SaveEndpoint(Models.Endpoint endpoint)
        {
            var request = new PutItemRequest();
            request.TableName = _DBSettings.Value.TableName;
            request.Item = endpoint.ConvertToDBDocument();

            var resp = await _Client.PutItemAsync(request);
            return resp.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
