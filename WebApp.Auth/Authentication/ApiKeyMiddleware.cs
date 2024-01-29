using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Auth.Authentication
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _configuration = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var providedApiKey = context.Request.Headers[AuthConfig.ApiKeyHeader].FirstOrDefault();
            var isValid = IsValidApiKey(providedApiKey);

            if (!isValid)
            {
                //Console.WriteLine("not valid");
                await GenerateResponse(context, 401, "Invalid Authentication");
                return;
            }

            await _next(context);

        }

        private async Task GenerateResponse(HttpContext context, int httpStatusCode, string message)
        {
            context.Response.StatusCode = httpStatusCode;
            await context.Response.WriteAsync(message);
        }

        private bool IsValidApiKey(string? providedApiKey)
        {
            if (string.IsNullOrWhiteSpace(providedApiKey)) { return false; }

            // install Microsoft.Extensions.Configuration.Binder and the method will be available.
            var validApiKey = _configuration.GetValue<string>(AuthConfig.AuthSection);

            return string.Equals(validApiKey, providedApiKey, StringComparison.Ordinal);

        }
    }
}
