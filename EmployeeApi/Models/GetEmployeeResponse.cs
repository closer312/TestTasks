using System.Runtime.Serialization;

namespace EmployeeAPI.Models;

[DataContract]
public class GetEmployeeResponse
{
    [DataMember]
    public Guid Id { get; set; }
    [DataMember]
    public string Name { get; set; } = string.Empty;
    [DataMember]
    public string Department { get; set; } = string.Empty;
}
