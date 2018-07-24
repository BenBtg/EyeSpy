using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;

namespace EyeSpy.Service.Authentication
{
    public class ApiKeyAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ApiKey";
        public string Scheme => DefaultScheme;
        public StringValues ApiKey { get; set; }
    }
}