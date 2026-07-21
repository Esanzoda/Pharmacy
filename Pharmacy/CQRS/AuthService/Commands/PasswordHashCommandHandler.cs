using MediatR;

namespace Pharmasy.CQRS.AuthService.Commands;

public record PasswordHashCommand(
    string Password) : IRequest<string>;
public class PasswordHashCommandHandler:IRequestHandler<PasswordHashCommand,string>
{

    public async Task<string> Handle(PasswordHashCommand request, CancellationToken cancellationToken)
    {
        return BCrypt.Net.BCrypt.HashPassword(request.Password);
    }
}