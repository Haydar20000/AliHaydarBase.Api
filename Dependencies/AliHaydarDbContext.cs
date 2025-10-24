using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.HelperFunctions.SeedConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Dependencies
{
    public class AliHaydarDbContext(DbContextOptions<AliHaydarDbContext> options) : IdentityDbContext<User, Role, string>(options)
    {
        public DbSet<RefreshTokenEntry> RefreshTokens { get; set; }
        public DbSet<ClaimDefinition> ClaimDefinitions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ClaimConfiguration());
        }
    }
}