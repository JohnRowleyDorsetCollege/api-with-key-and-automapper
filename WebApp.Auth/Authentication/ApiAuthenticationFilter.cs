using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Auth.Authentication
{
    public class ApiAuthenticationFilter : IAsyncAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public ApiAuthenticationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var providedApiKey = context.HttpContext.Request.Headers[AuthConfig.ApiKeyHeader].FirstOrDefault();

            var isValid = IsValidApiKey(providedApiKey);

            if (!isValid)
            {
                context.Result = new UnauthorizedObjectResult("Invalid Authentication");
                return;
               
            }

            
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
