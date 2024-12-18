using System.Runtime.Serialization;

namespace EmployeeAPI.Entities;

[DataContract]
public class Employee
{
    [DataMember]
    public Guid Id { get; set; } = Guid.NewGuid();
    [DataMember]
    public string Name { get; set; } = string.Empty;
    [DataMember]
    public string Department { get; set; } = string.Empty;
}