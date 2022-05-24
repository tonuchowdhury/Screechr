using Microsoft.EntityFrameworkCore;
using Screechr.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screechr.Repository
{
    public class ScreechrContext : DbContext
    {
        public ScreechrContext(DbContextOptions<ScreechrContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Screech> Screeches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(b => b.UserName)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(b => b.SecretToken)
                .IsUnique();
        }
    }
}
