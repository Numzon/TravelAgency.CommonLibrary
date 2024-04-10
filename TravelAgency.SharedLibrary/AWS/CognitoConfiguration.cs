using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TravelAgency.SharedLibrary.Enums;
using TravelAgency.SharedLibrary.Models;

namespace TravelAgency.SharedLibrary.AWS;
public static class CognitoConfiguration
{
    public static IServiceCollection AddAuthenticationAndJwtConfiguration(this IServiceCollection services, AwsCognitoSettingsDto settings)
    {
        var cognitoSigningKeys = GetCognitoSigningKeys(settings);

        services
            .AddAuthentication(
                o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(
                o =>
                {
                    o.Authority = settings.AuthorityUrl;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        AudienceValidator = (_, token, parameters) => parameters.ValidAudience
                            == (token as JwtSecurityToken)?.Claims.SingleOrDefault(c => c.Type == AwsTokenNames.ClientId)?.Value,
                        IssuerSigningKeys = cognitoSigningKeys,
                        ValidIssuer = settings.AuthorityUrl,
                        ValidAudience = settings.ClientId,
                        ValidAlgorithms = new[] { "RS256" },
                        ValidateIssuerSigningKey = true
                    };
                });

        return services;
    }


    public static IServiceCollection AddAuthorizationWithPolicies(this IServiceCollection services)
    {
        services
            .AddAuthorization(
                o =>
                {
                    o.AddPolicy(
                        PolicyNames.ClientPolicy,
                        p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.ClientAccount));

                    o.AddPolicy(
                        PolicyNames.TravelAgencyPolicy,
                        p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.TravelAgencyAccount));

                    o.AddPolicy(
                        PolicyNames.EmployeePolicy,
                        p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.Employee));

                    o.AddPolicy(
                       PolicyNames.TravelManagerPolicy,
                       p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.Employee, CognitoGroups.TravelManager));

                    o.AddPolicy(
                       PolicyNames.FleetManagerPolicy,
                       p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.Employee, CognitoGroups.FleetManager));

                    o.AddPolicy(
                       PolicyNames.HumanResourceManagerPolicy,
                       p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.Employee, CognitoGroups.HumanResourcesManager));

                    o.AddPolicy(
                       PolicyNames.FinancialResourceManagerPolicy,
                       p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.Employee, CognitoGroups.FinancialManager));
                });

        return services;
    }

    private static async Task<IList<SecurityKey>> GetCognitoSigningKeysAsync(AwsCognitoSettingsDto settings)
    {
        using var httpClient = new HttpClient();

        var cognitoSigningKeys = new JsonWebKeySet(await httpClient.GetStringAsync(settings.JwtKeysUrl)).GetSigningKeys();

        return cognitoSigningKeys;
    }

    private static IList<SecurityKey> GetCognitoSigningKeys(AwsCognitoSettingsDto settings)
    {
        return GetCognitoSigningKeysAsync(settings).ConfigureAwait(false).GetAwaiter().GetResult();
    }
}
