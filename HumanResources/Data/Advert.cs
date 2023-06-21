using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResources.Data
{
    public class Advert
    {
        public Guid Id { get; set; }

        [Display(Name = "Ünvan")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [MinLength(4, ErrorMessage = "{0} alanı en az {1} karakter olmalıdır.")]
        public required string Appellation { get; set; }

        
        [Display(Name = "Şirket Logo")]
        public  string? Logo { get; set; }

        
        [Display(Name = "Ülke Bayrağı")]
        public  string? Flag { get; set; }


       
       

        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
       
        [Display(Name = "Verilecek Maaş")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [MinLength(4, ErrorMessage = "{0} alanı en az {1} karakter olmalıdır.")]
        [Display(Name = "Aranan Nitelikler")]
        public required string Qualifications { get; set; }
        [NotMapped]
        public required string SalaryText { get; set; }
        public virtual Guid UserId { get; set; }
       
        [Display(Name = "Şirket Logo")]
        [NotMapped]
        public IFormFile? LogoFile { get; set; }
       
        [Display(Name = "Ülke Bayrağı")]
        [NotMapped]
        public IFormFile? FlagFile { get; set; }
        public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;    
        public virtual AppUser? CreatorUser { get; set; }
       
        public virtual Category? Category { get; set; }
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]      
        [Display(Name = "Aranan Departman")]
        public Guid CategoryId { get; set; }

       
    }
    public class AdvertEntityTypeConfiguration : IEntityTypeConfiguration<Advert>
    {
        public void Configure(EntityTypeBuilder<Advert> builder)
        {
            builder
                .HasIndex(p => p.Appellation)
                .IsUnique(false);
            builder
                .Property(p => p.Appellation)
                .IsRequired()
                .HasMaxLength(450);
            builder
                .Property(p => p.Logo)
                .IsUnicode();
            builder
                .Property(p => p.Flag)
                .IsUnicode();
            builder
                .Property(p => p.Salary)
                .HasPrecision(18, 4);

        }
    }
}