using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using TravelAgency.SharedLibrary.Enums;

namespace TravelAgency.SharedLibrary.Swagger;
public static class SwaggerConfiguration
{
    public static IServiceCollection AddAndConfigureSwagger(this IServiceCollection services, string serviceName)
    {
        return services.AddSwaggerGen(
            c =>
            {
                c.CustomSchemaIds(t => t.FullName!.Replace('+', '_'));
                c.SwaggerDoc("v1", new OpenApiInfo { Title = serviceName, Version = "v1" });

                c.AddSecurityDefinition(
                    JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        BearerFormat = "jwt",
                        Description = "Please enter your access token.",
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>(JwtBearerDefaults.AuthenticationScheme);
                c.EnableAnnotations();

                c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
                c.MapType<TimeOnly>(() => new OpenApiSchema { Type = "string", Pattern = TravelAgencyDateFormat.TimePattern });
                c.MapType<TimeSpan>(() => new OpenApiSchema { Type = "string", Pattern = TravelAgencyDateFormat.TimePattern });
            });
    }
}
