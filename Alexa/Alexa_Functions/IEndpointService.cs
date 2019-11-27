using Alexa_Functions.Models;
using System.Threading.Tasks;

namespace Alexa_Functions
{
    public interface IEndpointService
    {
        Task<bool> SaveEndpoint(Endpoint endpoint);
        Task<Endpoint> GetEndpoint(string userId);
    }
}
