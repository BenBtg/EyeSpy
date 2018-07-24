using System;
using EyeSpy.Service.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace EyeSpy.Service.Extensions
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKeyAuth(this AuthenticationBuilder builder, Action<ApiKeyAuthOptions> configureOptions)
        {
            return builder.AddScheme<ApiKeyAuthOptions, ApiKeyAuthHandler>(ApiKeyAuthOptions.DefaultScheme, configureOptions);
        }
    }
}