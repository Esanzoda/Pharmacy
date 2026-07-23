using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Auth.Commands;

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
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);
        if (refreshToken is null)
        {
            throw new RecourseNotFoundException("Invalid refresh token");
        }

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new BusinessException("Refresh token expired");
        }

        if (refreshToken.IsDeleted)
        {
            throw new RecourseNotFoundException("Refresh token not found or already deleted ");
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
            Token = await mediator.Send(new GenerateRefreshTokenCommand(), cancellationToken),
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