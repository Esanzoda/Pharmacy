using System.Security.Cryptography;
using MediatR;

namespace Pharmasy.Services.AuthService.Command;

public record GenereteRefreshTokenCommand : IRequest<string>;
public class GenereteRefreshTokenHendler:IRequestHandler<GenereteRefreshTokenCommand,string>
{
    public async Task<string> Handle(GenereteRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}