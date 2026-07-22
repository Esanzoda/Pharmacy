using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Auth.Commands;

public record RegisterCommand(
    CustomerRequest Request) : IRequest<CustomerResponse>;

public class RegisterHandler(
    IMapper mapper,
    IMediator mediator, 
    IApplicationDbContext dbContext)
    : IRequestHandler<RegisterCommand, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingCustomer = await dbContext.Customers
            .AnyAsync(x => x.Email == request.Request.Email, cancellationToken);
        if (existingCustomer)
        {
            throw new ResourseIsAlredyExistException("Customer already exist");
        }

        var passwordHash = await mediator.Send(new PasswordHashCommand(request.Request.Password), cancellationToken);
        var newCustomer = mapper.Map<Models.Domain.Customer>(request.Request);
        newCustomer.PasswordHash = passwordHash;
        await dbContext.Customers
            .AddAsync(newCustomer, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<CustomerResponse>(newCustomer);
    }
}