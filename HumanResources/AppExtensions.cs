using HumanResources.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HumanResources
{
    public static class AppExtensions
    {
        public static IApplicationBuilder UseHumanResources(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            context.Database.Migrate();
            new[]
        {
            new AppRole{ Name = "Administrators"},
          
        }
        .ToList()
        .ForEach(role =>
        {
            if (!roleManager.RoleExistsAsync(role.Name).Result)
            {
                var result = roleManager.CreateAsync(role).Result;
            }
        });

            var user = new AppUser
            {
                Name = configuration.GetValue<string>("DefaultUser:Name"),
                UserName = configuration.GetValue<string>("DefaultUser:Email"),
                Email = configuration.GetValue<string>("DefaultUser:Email"),
                EmailConfirmed = true
            };

            userManager.CreateAsync(user, configuration.GetValue<string>("DefaultUser:Password")).Wait();
            userManager.AddToRoleAsync(user, "Administrators").Wait();
            userManager.AddClaimAsync(user, new Claim("Name", user.Name)).Wait();
            return app;
        }

    }
}
