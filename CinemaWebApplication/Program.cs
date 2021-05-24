using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                RoleManager<IdentityRole> manager = services.GetService<RoleManager<IdentityRole>>();
                if (!manager.RoleExistsAsync("Admin").Result)
                {
                    var result = manager.CreateAsync(new IdentityRole("Admin")).Result;
                }

                if (!manager.RoleExistsAsync("Client").Result)
                {
                    var result = manager.CreateAsync(new IdentityRole("Client")).Result;
                }

                UserManager<IdentityUser> userManager = services.GetService<UserManager<IdentityUser>>();

                var user = userManager.FindByEmailAsync("admin@gmail.com").Result;
                if (user != null)
                {
                    if (!userManager.IsInRoleAsync(user, "Admin").Result)
                    {
                        var addResult = userManager.AddToRoleAsync(user, "Admin").Result;
                    }
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
