using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Pharmasy.Services.AuthService.Query;

public record GenerateTokenQueri(Models.Domain.Customer Request) : IRequest<string>;

public class GenerateTokenHendler : IRequestHandler<GenerateTokenQueri, string>
{
    private readonly IConfiguration _configuration;

    public GenerateTokenHendler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> Handle(GenerateTokenQueri request, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, request.Request.Id.ToString()),
            new Claim(ClaimTypes.Email, request.Request.Email),
            new Claim(ClaimTypes.Role, request.Request.Role.ToString())
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer:
            _configuration["JwtSettings:Issuer"],
            audience:
            _configuration["JwtSettings:Audience"],
            claims: claims,
            expires:
            DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(
                    _configuration["JwtSettings:AccessTokenExpirationMinutes"])),
            signingCredentials:
            credentials
        );
        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}