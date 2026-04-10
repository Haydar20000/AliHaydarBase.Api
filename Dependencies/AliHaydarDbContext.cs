using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.Core.Models.Blogs;
using AliHaydarBase.Api.Core.Models.Members;
using AliHaydarBase.Api.HelperFunctions.SeedConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Dependencies
{
    public class AliHaydarDbContext(DbContextOptions<AliHaydarDbContext> options) : IdentityDbContext<User, Role, string>(options)
    {
        // Identity-related DbSets
        public DbSet<RefreshTokenEntry> RefreshTokens { get; set; }
        public DbSet<ClaimDefinition> ClaimDefinitions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        // Blog-related DbSets
        public DbSet<Blogs> Blogs { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<BlogImages> BlogImages { get; set; }

        public DbSet<Member> Members { get; set; }
        public DbSet<IdCardTemplate> IdCardTemplates { get; set; }
        public DbSet<PrintHistory> PrintHistories { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ClaimConfiguration());

            // // this is for test we dont need all 
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
            modelBuilder.ApplyConfiguration(new IdCardTemplateConfiguration());
            modelBuilder.ApplyConfiguration(new PrintHistoryConfiguration());
        }
    }
}