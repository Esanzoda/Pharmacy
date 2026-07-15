using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.IdentityModel.Tokens;


namespace Pharmasy.Services.Cart.AuthService.Command;

public record GenerateTokenCommand(Models.Domain.Customer Request) : IRequest<string>;

public class GenerateTokenHendler : IRequestHandler<GenerateTokenCommand, string>
{
    private readonly IConfiguration _configuration;

    public GenerateTokenHendler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>()
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                request.Request.Id.ToString()),
            new Claim(ClaimTypes.Email,
                request.Request.Email)
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer:
            _configuration["Jwt:Issuer"],
            audience:
            _configuration["Jwt:Audience"],
            claims: claims,
            expires:
            DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(
                    _configuration["Jwt:ExpireMinutes"])),
            signingCredentials:
            credentials
        );
        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}