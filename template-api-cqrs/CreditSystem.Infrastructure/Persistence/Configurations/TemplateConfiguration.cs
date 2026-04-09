using Template.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Template.Infrastructure.Persistence.Configurations;

public class TemplateConfiguration : IEntityTypeConfiguration<TemplateEntitie>
{
    public void Configure(EntityTypeBuilder<TemplateEntitie> builder)
    {
        builder.ToTable("Templates");

        builder.HasKey(e => e.Id);
    }
}
