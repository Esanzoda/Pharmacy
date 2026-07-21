using MediatR;

namespace Pharmasy.CQRS.AuthService.Queries;
public record PasswordVerifyQuery(
    string Password, 
    string PasswordHash) : IRequest<bool>;
public class PasswordVerifyQueryHandler:IRequestHandler<PasswordVerifyQuery,bool>
{
    public async Task<bool> Handle(PasswordVerifyQuery request, CancellationToken cancellationToken)
    {
        return BCrypt.Net.BCrypt.Verify(request.Password,request.PasswordHash);
    }
}