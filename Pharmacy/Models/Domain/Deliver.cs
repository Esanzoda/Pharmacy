using System.Reflection.Metadata.Ecma335;

namespace Pharmasy.Models.Domain;

public class Deliver:BaseEntity
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string  Email { get; set; }
    public string  Password { get; set; }
    //public string PasswordHash { get; set; }
} 