using AutoMapper;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Employee;

public class EmployeDiBase
{
    protected readonly IEmployeeRepository EmployeeRepository;
    protected readonly IMapper Mapper;

    public EmployeDiBase(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        EmployeeRepository = employeeRepository;
        Mapper = mapper;
    }
    public EmployeDiBase(IEmployeeRepository employeeRepository)
    {
        EmployeeRepository = employeeRepository;
    }
}