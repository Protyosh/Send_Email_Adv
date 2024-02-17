
using Assignment.Models;
using Microsoft.EntityFrameworkCore;

    namespace AspNetEmailExample.Models
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<Recipient> Recipients { get; set; }
            public DbSet<SentEmail> SentEmails { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<SentEmail>()
                    .HasOne(e => e.Recipient)
                    .WithMany(r => r.SentEmails)
                    .HasForeignKey(e => e.RecipientId);
            }
        }
    }

