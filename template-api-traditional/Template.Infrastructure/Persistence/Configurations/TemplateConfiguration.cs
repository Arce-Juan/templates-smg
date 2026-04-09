using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistence.Configurations;

public class TemplateConfiguration : IEntityTypeConfiguration<TemplateEntity>
{
    public void Configure(EntityTypeBuilder<TemplateEntity> builder)
    {
        builder.ToTable("Templates");
        builder.HasKey(c => c.Id);
    }
}
