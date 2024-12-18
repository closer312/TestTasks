using EmployeeAPI.Entities;
using EmployeeAPI.Interfaces;
using EmployeeAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeRequest request)
        {
            await _employeeService.AddEmployeeAsync(request);
            return Ok("Employee added");
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetEmployeesByName([FromQuery] string name)
        {
            var employees = await _employeeService.GetEmployeesByNameAsync(name);
            return Ok(employees);
        }

        [HttpGet("GetByDepartment")]
        public async Task<IActionResult> GetEmployeesByDepartment([FromQuery] string department)
        {
            var employees = await _employeeService.GetEmployeesByDepartmentAsync(department);
            return Ok(employees);
        }
    }
}
