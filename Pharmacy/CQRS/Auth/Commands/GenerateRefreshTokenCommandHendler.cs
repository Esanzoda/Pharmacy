using System.Security.Cryptography;
using MediatR;

namespace Pharmacy.CQRS.Auth.Commands;

public record GenerateRefreshTokenCommand : IRequest<string>;

public class GenerateRefreshTokenHandler : IRequestHandler<GenerateRefreshTokenCommand, string>
{
    public Task<string> Handle(GenerateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Task.FromResult(Convert.ToBase64String(randomBytes));
    }
}