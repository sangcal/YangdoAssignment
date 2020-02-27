using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using YangdoDTO;

namespace YangdoDAO
{
    public class YangdoDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=yangdo;User ID=sa;Password=1234;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
            .Property(b => b.RegisterDate)
            .HasDefaultValueSql("getdate()")
            .IsRequired(false);


            modelBuilder.Entity<Task>()
            .Property(b => b.RegisterDate)
            .HasDefaultValueSql("getdate()")
            .IsRequired(false);

            modelBuilder.Entity<TimeSheet>(entity =>
            {
                entity.Property("RegisterDate")
                .HasDefaultValueSql("getdate()")
                .IsRequired(false);

                // to include relevant object in many-to-many relationships
                entity.HasOne(x => x.Person)
                .WithMany(x => x.TimeSheets)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Task)
                .WithMany(x => x.TimeSheets)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public DbSet<Person> People { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<TimeSheet> TimeSheets { get; set; }
    }
}
