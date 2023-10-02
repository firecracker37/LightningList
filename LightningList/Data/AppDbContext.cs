using LightningList.Models;
using Microsoft.EntityFrameworkCore;

namespace LightningList.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<TodoList> TodoLists { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // If you have other base entities or abstract classes, you might want to add a condition
                // to check if the entity type has the CreatedAt and UpdatedAt properties
                if (typeof(BaseModel).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.Name)
                        .Property<DateTime>("CreatedAt")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .ValueGeneratedOnAdd();

                    modelBuilder.Entity(entityType.Name)
                        .Property<DateTime>("UpdatedAt")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .ValueGeneratedOnAddOrUpdate();
                }
            }
        }

    }

}
