using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Exception;
using Pharmasy.Interfaces;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.CQRS.Auth.Commands;

public record ReGenerateRefreshTokenCommand(
    string RefreshToken) : IRequest<LoginResponse>;

public class ReGenerateRefreshTokenHandler(
    IApplicationDbContext dbContext,
    IMediator mediator)
    : IRequestHandler<ReGenerateRefreshTokenCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(ReGenerateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var refreshToken = await dbContext.RefreshTokens
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken,cancellationToken);
        if (refreshToken is null)
        {
            throw new ResourseNotFoundException("Invalid refresh token");
        }

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new BusinessException("Refresh token expired");
        }

        if (refreshToken.IsDeleted)
        {
            throw new ResourseNotFoundException("Refresh tocen not found or alredy deleted ");
        }

        refreshToken.ExpiresAt = now;
        refreshToken.UpdateAt = now;
        refreshToken.IsDeleted = true;

        var newRefreshToken = new RefreshToken()
        {
            Customer = refreshToken.Customer,
            CreatedAt = now,
            UpdateAt = now,
            ExpiresAt = now.AddMinutes(3),
            Token = await mediator.Send(new GenereteRefreshTokenCommand(), cancellationToken),
           
        };
        await dbContext.RefreshTokens
            .AddAsync(newRefreshToken, cancellationToken);
        var newAccessToken = await mediator.Send(new GenerateTokenCommand(refreshToken.Customer), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new LoginResponse()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token
        };
    }
}