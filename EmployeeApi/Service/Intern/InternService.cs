using EmployeeApi.Data;
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
            return await _context.Interns.ToListAsync();
        }

        public async Task<InternEntity?> GetById(int id)
        {
            return await _context.Interns.FindAsync(id);
        }

        public async Task<InternEntity> Create(InternEntity intern)
        {
            _context.Interns.Add(intern);

            await _context.SaveChangesAsync();

            return intern;
        }

        public async Task<InternEntity> Update(int id, InternEntity intern)
        {
            var existingIntern = await _context.Interns.FindAsync(id);

            if (existingIntern == null)
            {
                throw new InvalidOperationException("Intern not found");
            }

            _context.Entry(existingIntern)
                .CurrentValues
                .SetValues(intern);

            await _context.SaveChangesAsync();

            return existingIntern;
        }

        public async Task<bool> Delete(int id)
        {
            var intern = await _context.Interns.FindAsync(id);

            if (intern == null)
            {
                return false;
            }

            _context.Interns.Remove(intern);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}