using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pharmacy.Infrastructure.Setting;

namespace Pharmacy.CQRS.Auth.Commands;

public record GenerateTokenCommand(Models.Domain.Customer Request) : IRequest<string>;

public class GenerateTokenCommandHandler(IOptionsMonitor<JwtOption> jwt) : IRequestHandler<GenerateTokenCommand, string>
{
    public Task<string> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, request.Request.Id.ToString()),
            new Claim(ClaimTypes.Email, request.Request.Email),
            new Claim(ClaimTypes.Role, request.Request.Role.ToString())
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt.CurrentValue.SecretKey));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwt.CurrentValue.Issuer,
            audience: jwt.CurrentValue.Audience,
            claims: claims,
            expires:
            DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(jwt.CurrentValue.AccessTokenExpirationMinutes)),
            signingCredentials:
            credentials
        );
        return Task.FromResult(new JwtSecurityTokenHandler()
            .WriteToken(token));
    }
}