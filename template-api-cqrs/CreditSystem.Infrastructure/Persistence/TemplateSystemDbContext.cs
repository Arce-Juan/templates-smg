using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistence;

public class TemplateSystemDbContext : DbContext
{
    public TemplateSystemDbContext(DbContextOptions<TemplateSystemDbContext> options)
        : base(options)
    {
    }

    public DbSet<TemplateEntitie> Templates => Set<TemplateEntitie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TemplateSystemDbContext).Assembly);
    }
}
