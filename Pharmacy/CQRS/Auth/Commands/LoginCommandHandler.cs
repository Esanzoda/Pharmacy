using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Auth.Queries;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Dto.Request;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Auth.Commands;

public record LoginCommand(
    LoginRequest Request) : IRequest<LoginResponse>;

public class LoginHandler(
    IMediator mediator,
    IApplicationDbContext dbContext) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(x => x.Email == request.Request.Email, cancellationToken);
        if (customer is null)
        {
            throw new ResourseNotFoundException("Customer not found");
        }

        var passwordCheck = await mediator.Send(new PasswordVerifyQuery(request.Request.Password, customer.PasswordHash), cancellationToken);
        if (!passwordCheck)
        {
            throw new BusinessException("Invalid email or password");
        }

        var accesToken = await mediator.Send(new GenerateTokenCommand(customer), cancellationToken);
        var newRefreshToken =
            new RefreshToken
            {
                CustomerId = customer.Id,
                Token = await mediator.Send(new GenereteRefreshTokenCommand(), cancellationToken),
                ExpiresAt = now
                    .AddDays(7),
                CreatedAt = now
            };
        await dbContext.RefreshTokens
            .AddAsync(newRefreshToken, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new LoginResponse()
        {
            AccessToken = accesToken,
            RefreshToken = newRefreshToken.Token
        };
    }
}