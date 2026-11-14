using DotNetEnv;
using GavinHsLogic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

namespace PersonalGUI.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load env
            Env.Load();
            string[] allowedCors = Environment.GetEnvironmentVariable("CORS_ALLOWED").Split("|");

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<DataAccessBridge>();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None; // Needed for cross-origin cookies
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy =>
                    {
                        policy.WithOrigins(allowedCors) //allowed cors
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials(); //Important for cookies
                    });
            });

            builder.Services.AddAuthorization();

            builder.Services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress!.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,      // 100 requests
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var projService = scope.ServiceProvider.GetRequiredService<DataAccessBridge>();

                await projService.GetAllProjects(); //calls db logic and populates the project lists
                await projService.GetAllJobs();     //calls db logic and populates the job lists
                await projService.GetResumay();     //calls db logic and populates the resumay
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection(); // Forces HTTPS immediately for a given request
                app.UseHsts();             // Tells the browser to never use HTTP again for x amount of time
            }

            app.UseCors("AllowReactApp");

            app.UseSession();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
