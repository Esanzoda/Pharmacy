using MediatR;
using Pharmasy.Exception;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Employee.Command;

public record DeleteEmployeCommand(long EmployeeId) : IRequest<bool>;

public class DeleteEmployeHendler : EmployeDiBase, IRequestHandler<DeleteEmployeCommand, bool>
{
    public DeleteEmployeHendler(IEmployeeRepository employeeRepository)
        : base(employeeRepository)
    {
    }

    public async Task<bool> Handle(DeleteEmployeCommand request, CancellationToken cancellationToken)
    {
        var delete = await EmployeeRepository.DeleteAsync(request.EmployeeId);
        if (delete == false)
        {
            throw new ResourseNotFoundException($"Employe with id {request.EmployeeId} not found");
        }

        await EmployeeRepository.SaveChangesAsync();
        return true;
    }
}