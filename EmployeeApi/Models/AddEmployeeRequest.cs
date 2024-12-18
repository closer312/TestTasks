using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EmployeeAPI.Models;

[DataContract]
public class AddEmployeeRequest
{
    [DataMember]
    [Required]
    public string Department {  get; set; } = string.Empty;
    [DataMember]
    [Required]
    public string Name {  get; set; } = string.Empty;
}
