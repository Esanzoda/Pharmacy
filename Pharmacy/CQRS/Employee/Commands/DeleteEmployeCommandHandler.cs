using MediatR;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Employee.Commands;

public record DeleteEmployeCommand(
    long EmployeeId
    ) : IRequest<bool>;

public class DeleteEmployeHandler(
    IApplicationDbContext dbContext
    ) :  IRequestHandler<DeleteEmployeCommand, bool>
{

    public async Task<bool> Handle(DeleteEmployeCommand request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FindAsync(request.EmployeeId, cancellationToken);
        if (employee is null)
        {
            throw new ResourseNotFoundException($"Employe with id {request.EmployeeId} not found");
        }

        dbContext.Employees.Remove(employee);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}