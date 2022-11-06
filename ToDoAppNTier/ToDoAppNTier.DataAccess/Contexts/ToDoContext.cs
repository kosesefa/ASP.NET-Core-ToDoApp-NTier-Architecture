using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using ToDoAppNTier.DataAccess.Configurations;
using ToDoAppNTier.Entities.Domains;

namespace ToDoAppNTier.DataAccess.Contexts
{
    public class ToDoContext:DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WorkConfiguration());
        }
        public DbSet<Work> Works { get; set; }
    }
}
