using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HumanResources.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.Data;

public class LogoSettings
{
    public Guid Id { get; set; }
    public string? AdminLogo { get; set; }
    [NotMapped]
    public IFormFile? AdminFile { get; set; }
    public string? WebLogo { get; set; }
    [NotMapped]
    public IFormFile? WebFile { get; set; }
}

public class LogoSettingsEntityTypeConfiguration : IEntityTypeConfiguration<LogoSettings>
{
    public void Configure(EntityTypeBuilder<LogoSettings> builder)
    {
    }
}
