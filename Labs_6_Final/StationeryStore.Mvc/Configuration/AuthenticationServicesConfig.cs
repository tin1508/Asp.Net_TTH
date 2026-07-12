using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace StationeryStore.Mvc.Configuration;

public static class AuthenticationServicesConfig
{
    public static IServiceCollection AddGoogleAuthentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication()
            .AddGoogleOpenIdConnect(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];

                googleOptions.NonceCookie.SameSite = SameSiteMode.Lax;
                googleOptions.NonceCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

                googleOptions.CorrelationCookie.SameSite = SameSiteMode.Lax;
                googleOptions.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

                googleOptions.ResponseType = OpenIdConnectResponseType.Code;
                googleOptions.ResponseMode = OpenIdConnectResponseMode.Query;
            });

        return services;
    }
}