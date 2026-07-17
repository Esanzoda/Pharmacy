using AutoMapper.Configuration.Conventions;
using Pharmasy.Models.Domain.Enum;

namespace Pharmasy.Models.Dto.Request;

public record CustomerRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
    public Role Role { get; set; }
}