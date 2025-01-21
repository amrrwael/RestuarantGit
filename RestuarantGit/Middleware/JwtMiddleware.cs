using Delivery.Resutruant.API.Services.Interfaces;
using System.Net;

namespace Delivery.Resutruant.API.Configrations
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IBlacklistService _blacklistService;

        public JwtMiddleware(RequestDelegate next, IBlacklistService blacklistService)
        {
            _next = next;
            _blacklistService = blacklistService;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Check if the token is blacklisted
            if (token != null && await _blacklistService.IsTokenBlacklistedAsync(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Token is blacklisted.");
                return;
            }

            await _next(context);
        }
    }

}


