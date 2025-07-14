using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Src.Domain.Entity;

namespace Src.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Aquí defines explícitamente el DbSet
        public DbSet<UserEntity> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<BaseEntity>();

            base.OnModelCreating(modelBuilder);

            var entityTypes = Assembly.GetExecutingAssembly()
                                      .GetTypes()
                                      .Where(t => 
                                        typeof(BaseEntity).IsAssignableFrom(t) &&
                                        t.IsClass &&
                                        !t.IsAbstract &&
                                        !t.IsGenericType
                                      );

            foreach (var entityType in entityTypes)
            {
                modelBuilder.Entity(entityType);
            }
        }
    }
}
