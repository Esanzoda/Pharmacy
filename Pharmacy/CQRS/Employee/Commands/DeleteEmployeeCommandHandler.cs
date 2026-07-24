using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Employee.Commands;

public record DeleteEmployeeCommand(long PharmacyId,
    long EmployeeId
) : IRequest<bool>;

public class DeleteEmployeeHandler(
    IApplicationDbContext dbContext) : IRequestHandler<DeleteEmployeeCommand, bool>
{
    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(
                x => x.Id == request.EmployeeId &&
                     x.PharmacyId == request.PharmacyId,
                cancellationToken);
        if (employee is null)
        {
            throw new RecourseNotFoundException($"Employee with id {request.EmployeeId} not found");
        }

        dbContext.Employees.Remove(employee);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}