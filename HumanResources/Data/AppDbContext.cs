using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace HumanResources.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        public virtual DbSet<Advert> Adverts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Recourse> Recourses { get; set; }
        public virtual DbSet<LogoSettings> LogoSettings { get; set; }
    }

}
