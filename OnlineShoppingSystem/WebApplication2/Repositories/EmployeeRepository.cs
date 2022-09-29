using Microsoft.EntityFrameworkCore;
using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShoppingSystem.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly FO_DBContext _context;
        public EmployeeRepository(FO_DBContext context)
        {
            _context = context;
        }


        public async Task<Employee> CreateEmployee(Employee Employee)
        {
            _context.Employees.Add(Employee);
            await _context.SaveChangesAsync();

            return Employee;
        }

        public async Task DeleteEmployee(int id)
        {
            var EmployeeDelete = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(EmployeeDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task UpdateEmployee(Employee Employee)
        {
            _context.Entry(Employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
