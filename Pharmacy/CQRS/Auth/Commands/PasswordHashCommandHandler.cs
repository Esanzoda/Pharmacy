using MediatR;

namespace Pharmacy.CQRS.Auth.Commands;

public record PasswordHashCommand(
    string Password) : IRequest<string>;

public class PasswordHashCommandHandler : IRequestHandler<PasswordHashCommand, string>
{
    public Task<string> Handle(PasswordHashCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(BCrypt.Net.BCrypt.HashPassword(request.Password));
    }
}