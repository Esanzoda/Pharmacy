using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Auth.Commands;

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
        var newCustomer = mapper.Map<Pharmasy.Models.Domain.Customer>(request.Request);
        newCustomer.PasswordHash = passwordHash;
        await dbContext.Customers
            .AddAsync(newCustomer, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<CustomerResponse>(newCustomer);
    }
}