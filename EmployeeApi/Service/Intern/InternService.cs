using EmployeeApi.Data;
using EmployeeApi.ViewModel.Intern;
using Microsoft.EntityFrameworkCore;
using InternEntity = EmployeeApi.Model.Intern.Intern;

namespace EmployeeApi.Service.Intern
{
    public class InternService : IInternService
    {
        private readonly ApplicationDbContext _context;

        public InternService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InternEntity>> GetAll()
        {
            return await _context.Interns
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<InternEntity?> GetById(int id)
        {
            return await _context.Interns.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<InternEntity> Create(InternEntity intern)
        {
            _context.Interns.Add(intern);
            await _context.SaveChangesAsync();
            return intern;
        }

        public async Task<InternEntity> Update(int id, InternEntity intern)
        {
            var existingIntern = await _context.Interns.FirstOrDefaultAsync(i => i.Id == id)
                ?? throw new KeyNotFoundException($"Intern {id} was not found.");

            existingIntern.Name = intern.Name;
            existingIntern.Description = intern.Description;
            await _context.SaveChangesAsync();
            return existingIntern;
        }

        public async Task<bool> Delete(int id)
        {
            var intern = await _context.Interns.FirstOrDefaultAsync(i => i.Id == id);
            if (intern is null)
            {
                return false;
            }

            intern.IsDeleted = true;
            intern.IsActive = false;
            intern.DeletedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<InternDropdownViewModel>> GetDropdown(string query)
        {
            var interns = _context.Interns.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                interns = interns.Where(i => i.Name.Contains(query));
            }

            return await interns
                .OrderBy(i => i.Name)
                .Select(i => new InternDropdownViewModel
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .ToListAsync();
        }
    }
}