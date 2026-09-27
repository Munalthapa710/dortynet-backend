using EmployeeApi.Model.AssignTask;
using EmployeeApi.Model.Client;
using EmployeeApi.Model.Common;
using EmployeeApi.Model.Department;
using EmployeeApi.Model.Employee;
using EmployeeApi.Model.Intern;
using Microsoft.EntityFrameworkCore;
using InternEntity = EmployeeApi.Model.Intern.Intern;
namespace EmployeeApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;

        public DbSet<AssignedTask> AssignedTasks { get; set; } = null!;

        public DbSet<Department> Departments { get; set; } = null!;

        public DbSet<Client> Clients { get; set; } = null!;

        public DbSet<Intern> Interns { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {  // to configure database rules and relationships between entities using fluent API 
            modelBuilder.Entity<Employee>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<AssignedTask>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<Department>().HasQueryFilter(d => !d.IsDeleted);
            modelBuilder.Entity<Client>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Intern>().HasQueryFilter(i => !i.IsDeleted);

            modelBuilder.Entity<Employee>() // use fluent API to configure the Salary property with precision and scale
                .Property(e => e.Salary) // configure the Salary property of the Employee entity
                .HasPrecision(18, 2); // set the precision to 18 and scale to 2

            modelBuilder.Entity<Intern>() // use fluent API to configure the relationship between Intern and Employee
                .HasMany(i => i.Employees); // configure the Employees navigation property of the Intern entity

            modelBuilder.Entity<Employee>() // use fluent API to configure the relationship between Employee and Department
                .HasOne(e => e.Department) // configure the Department navigation property of the Employee entity
                .WithMany(d => d.Employees) // configure the Employees navigation property of the Department entity
                .HasForeignKey(e => e.DepartmentId); // configure the foreign key property of the Employee entitys



            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditValues();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditValues()
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.AddedOn = utcNow;
                    entry.Entity.IsActive = true;
                    entry.Entity.IsDeleted = false;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedOn = utcNow;
                }
            }
        }
    }
}
