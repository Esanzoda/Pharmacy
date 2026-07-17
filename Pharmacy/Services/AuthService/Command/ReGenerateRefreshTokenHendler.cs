using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;
using Pharmasy.Services.AuthService.Query;

namespace Pharmasy.Services.AuthService.Command;

public record ReGenerateRefreshTokenComman(string RefreshToken) : IRequest<LoginResponse>;

public class ReGenerateRefreshTokenHendler : IRequestHandler<ReGenerateRefreshTokenComman, LoginResponse>
{
    private readonly IRefreshToken _refreshTokenRepo;
    private readonly IMediator _mediator;

    public ReGenerateRefreshTokenHendler(IRefreshToken refreshTokenRepo, IMediator mediator)
    {
        _refreshTokenRepo = refreshTokenRepo;
        _mediator = mediator;
    }

    public async Task<LoginResponse> Handle(ReGenerateRefreshTokenComman request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepo.GetAsync(request.RefreshToken);
        if (refreshToken is null)
        {
            throw new ResourseNotFoundException("Invalid refresh token");
        }

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new BusinessException("Refresh token expired");
        }

        if (!refreshToken.IsDeleted)
        {
            throw new ResourseNotFoundException("Refresh tocen not found or alredy deleted ");
        }

        refreshToken.ExpiresAt = DateTime.UtcNow;
        refreshToken.UpdateAt = DateTime.UtcNow;
        refreshToken.IsDeleted = true;
        await _refreshTokenRepo.UpdateAsync(refreshToken);
        await _refreshTokenRepo.SaveChangesAsync();

        new RefreshToken()
        {
            CustomerId = refreshToken.CustomerId,
            CreatedAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(3),
            Token = await _mediator.Send(new GenereteRefreshTokenCommand()),
            Customer = refreshToken.Customer
        };
        var newAccessToken = await _mediator.Send(new GenerateTokenQueri(refreshToken.Customer));
        return new LoginResponse()
        {
            AccessToken = newAccessToken,
            RefreshToken = refreshToken.Token
        };

    }
}