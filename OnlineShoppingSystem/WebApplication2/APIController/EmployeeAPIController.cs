using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Interfaces;
using OnlineShoppingSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShoppingSystem.APIController
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeAPIController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeAPIController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _employeeRepository.GetAllEmployees();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            return await _employeeRepository.GetEmployeeById(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Employee>> PostEmployee([FromBody] Employee Employee)
        {
            var newEmployee = await _employeeRepository.CreateEmployee(Employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.Id }, newEmployee);
        }

        [HttpPut]
        public async Task<ActionResult> PutEmployee(int id,[FromBody] Employee Employee)
        {
            if (id != Employee.Id)
            {
                return BadRequest();
            }

            await _employeeRepository.UpdateEmployee(Employee);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var deleteEmployee = await _employeeRepository.GetEmployeeById(id);
            if (deleteEmployee == null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteEmployee(deleteEmployee.Id);
            return NoContent();
        }
    }
}
