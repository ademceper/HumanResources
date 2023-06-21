using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.Data;

public class Category
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public virtual AppUser? CreatorUser { get; set; }
    public Guid UserId { get; set; }
    public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Advert> Adverts { get; set; } = new HashSet<Advert>();
}
public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasIndex(a => a.Name)
            .IsUnique(false);
        builder
            .Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(450);
        builder
               .HasMany(p => p.Adverts)
               .WithOne(p => p.Category)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}