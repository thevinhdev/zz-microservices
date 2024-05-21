using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IOIT.Identity.Api.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var secret = configuration.GetValue("JwtConfig:SecretKey", "PDv7DrqznYL6nv7DrqzjnQYO9JxIsWdcjnQYL6nu0f");
            var audience = configuration.GetValue("JwtConfig:Audience", "localhost");
            var issuer = configuration.GetValue("JwtConfig:Issuer", "localhost");
            var requireHttpsMetadata = configuration.GetValue("JwtConfig:RequireHttpsMetadata", false);
            var key = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = requireHttpsMetadata;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = configuration.GetValue("JwtConfig:TokenValidationParameter:ValidateIssuer", true),
                    ValidateAudience = configuration.GetValue("JwtConfig:TokenValidationParameter:ValidateAudience", true),
                    ValidateLifetime = configuration.GetValue("JwtConfig:TokenValidationParameter:ValidateLifetime", true),
                    ValidateIssuerSigningKey = configuration.GetValue("JwtConfig:TokenValidationParameter:ValidateIssuerSigningKey", true),
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ClockSkew = TimeSpan.Zero
                };
            });

            //services.AddAuthorization(config =>
            //{
            //    config.AddPolicy(Constants.Policies.SUPERADMIN, UserPolicy.PolicySuperAdmin());
            //    config.AddPolicy(Constants.Policies.COUNTRYADMIN, UserPolicy.PolicyCountryAdmin());
            //    config.AddPolicy(Constants.Policies.SUPERCOUNTRYADMIN, UserPolicy.PolicySuperCountryAdmin());
            //    config.AddPolicy(Constants.Policies.USER, UserPolicy.PolicyUser());
            //});

            return services;
        }
    }
}
