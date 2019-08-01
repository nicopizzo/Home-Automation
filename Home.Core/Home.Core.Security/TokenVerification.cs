using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Home.Core.Security
{
    public class TokenVerification
    {
        private readonly RequestDelegate _next;
        private readonly string _validatorKey;

        public TokenVerification(RequestDelegate next, string validatorKey)
        {
            _next = next;
            _validatorKey = validatorKey;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers.GetCommaSeparatedValues("Token").FirstOrDefault();
            if(token != _validatorKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token not valid.");
                return;
            }
            await _next(context);
        }
    }
}
