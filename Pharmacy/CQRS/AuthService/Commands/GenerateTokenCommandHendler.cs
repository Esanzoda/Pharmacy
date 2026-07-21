using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Pharmasy.CQRS.AuthService.Commands;

public record GenerateTokenCommand(Pharmasy.Models.Domain.Customer Request) : IRequest<string>;

public class GenerateTokenCommandHandler(IConfiguration configuration) : IRequestHandler<GenerateTokenCommand, string>
{
   
    public async Task<string> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, request.Request.Id.ToString()),
            new Claim(ClaimTypes.Email, request.Request.Email),
            new Claim(ClaimTypes.Role, request.Request.Role.ToString())
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer:
            configuration["JwtSettings:Issuer"],
            audience:
            configuration["JwtSettings:Audience"],
            claims: claims,
            expires:
            DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(
                    configuration["JwtSettings:AccessTokenExpirationMinutes"])),
            signingCredentials:
            credentials
        );
        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}