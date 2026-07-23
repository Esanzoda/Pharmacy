using MediatR;

namespace Pharmacy.CQRS.Auth.Queries;

public record PasswordVerifyQuery(
    string Password,
    string PasswordHash) : IRequest<bool>;

public class PasswordVerifyQueryHandler : IRequestHandler<PasswordVerifyQuery, bool>
{
    public Task<bool> Handle(PasswordVerifyQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(BCrypt.Net.BCrypt.Verify(request.Password, request.PasswordHash));
    }
}