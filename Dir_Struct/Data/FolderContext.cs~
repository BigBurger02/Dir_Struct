using Dir_Struct.Models;
using Microsoft.EntityFrameworkCore;

namespace Dir_Struct.Data
{
    public class FolderContext : DbContext
    {
        public FolderContext(DbContextOptions<FolderContext> options) : base(options)
        {
        }

        public DbSet<Folder_Entity> Folder_Entities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder_Entity>().ToTable("Folder_Entity");
        }
    }
}

