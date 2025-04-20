using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace TransactionsApi.Settings
{
    public static class AuthenticationExtentions
    {
        public static void ConfigureAuthentication(this IServiceCollection services, string secretKey)
        {
            var key = Encoding.ASCII.GetBytes(secretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opiton =>
            {
                opiton.RequireHttpsMetadata = false;
                opiton.SaveToken = true;
                opiton.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                opiton.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var cookie = context.Request.Cookies["auth_token"];
                        if (!string.IsNullOrEmpty(cookie))
                        {
                            context.Token = cookie;
                        }

                        return Task.CompletedTask;
                    },

                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var result = JsonSerializer.Serialize(new { error = "Usuário não autenticado" });
                        return context.Response.WriteAsync(result);
                    }

                };
            });


        }
    }
}
