using EmployeeApi.Data;
using EmployeeAPI.Entities;
using EmployeeAPI.Interfaces;
using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Services;

public class EmployeeService : IEmployeeService
{
    private readonly EmployeeDbContext _context;
    public EmployeeService(EmployeeDbContext context)
    {
        _context = context;
    }


    public async Task<Guid> AddEmployeeAsync(AddEmployeeRequest request)
    {
        var entity = new Employee { Name = request.Name, Department = request.Department };

        var newEntity = (await _context.Employees.AddAsync(entity)).Entity;

        await _context.SaveChangesAsync();
        return newEntity.Id;
    }

    public async Task<List<GetEmployeeResponse>> GetEmployeesByDepartmentAsync(string department)
    {
        var entities = await _context.Employees.Where(e => e.Department.ToLower() == department.ToLower()).ToListAsync();
        return FormResponse(entities);
    }

    public async Task<List<GetEmployeeResponse>> GetEmployeesByNameAsync(string name)
    {
        var entities = await _context.Employees.Where(e => e.Name.ToLower() == name.ToLower()).ToListAsync();
        return FormResponse(entities);
    }
    private static List<GetEmployeeResponse> FormResponse(List<Employee> employees)
    {
        return employees.Select(e => new GetEmployeeResponse
        {
            Id = e.Id,
            Department = e.Department,
            Name = e.Name
        }).ToList();
    }
}