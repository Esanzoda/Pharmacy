using System.Security.Cryptography;
using MediatR;

namespace Pharmacy.CQRS.Auth.Commands;

public record GenereteRefreshTokenCommand : IRequest<string>;
public class GenereteRefreshTokenHandler:IRequestHandler<GenereteRefreshTokenCommand,string>
{
    public async Task<string> Handle(GenereteRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}