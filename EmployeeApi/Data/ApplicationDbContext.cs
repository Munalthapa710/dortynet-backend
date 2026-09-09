using EmployeeApi.Model.AssignTask;
using EmployeeApi.Model.Client;
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
        {
            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Intern>()
                .HasMany(i => i.Employees);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
