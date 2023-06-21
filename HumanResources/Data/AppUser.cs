using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.Data
{
    public class AppUser : IdentityUser<Guid>
    {
        public required string Name { get; set; }
        public virtual ICollection<Advert> CreatedAdverts { get; set; } = new HashSet<Advert>();
        public virtual ICollection<Category> CreatedCategories { get; set; } = new HashSet<Category>();


    }

    public class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder
                .HasIndex(a => a.Name)
                .IsUnique(false);
            builder
                .HasMany(a => a.CreatedAdverts)
                .WithOne(a => a.CreatorUser)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
              .HasMany(a => a.CreatedCategories)
              .WithOne(a => a.CreatorUser)
              .HasForeignKey(a => a.UserId)
              .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
