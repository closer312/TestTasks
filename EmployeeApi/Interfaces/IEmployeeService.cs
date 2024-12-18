using System.ServiceModel;
using EmployeeAPI.Models;

namespace EmployeeAPI.Interfaces;

[ServiceContract]
public interface IEmployeeService
{
    [OperationContract]
    Task<Guid> AddEmployeeAsync(AddEmployeeRequest request);

    [OperationContract]
    Task<List<GetEmployeeResponse>> GetEmployeesByNameAsync(string name);

    [OperationContract]
    Task<List<GetEmployeeResponse>> GetEmployeesByDepartmentAsync(string department);
}
