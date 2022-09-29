using OnlineShoppingSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShoppingSystem.Interfaces
{
    public interface IEmployeeRepository
    {
        Task <IEnumerable<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(int id);
        Task<Employee> CreateEmployee(Employee Employee);
        Task UpdateEmployee(Employee Employee);
        Task DeleteEmployee(int id);
    }
}
