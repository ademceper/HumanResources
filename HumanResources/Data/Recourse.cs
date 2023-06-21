using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HumanResources.Data
{
    public class Recourse
    {
        public Guid Id { get; set; }

        [Display(Name = "Başvuran Kişi")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [MinLength(4, ErrorMessage = "{0} alanı en az {1} karakter olmalıdır.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [MinLength(10, ErrorMessage = "{0} alanı en az {1} karakter olmalıdır.")]
        [Display(Name = "Telefon Numarası")]
        public required string Number { get; set; }

        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [MinLength(4, ErrorMessage = "{0} alanı en az {1} karakter olmalıdır.")]
        [Display(Name = "Email Adresi")]
        public required string Email { get; set; }

        public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;




    }

    public class RecourseEntityTypeConfiguration : IEntityTypeConfiguration<Recourse>
    {
        public void Configure(EntityTypeBuilder<Recourse> builder)
        {
            builder
                .HasIndex(p => p.Name)
                .IsUnique(false);
            builder
                .Property(p => p.Name)
                .IsRequired();

        }
    }
}