using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Pharmasy.Exception;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart.AuthService.Command;

public record LoginCommand(LoginRequest Request) :  IRequest<LoginResponse>;
public class LoginHendler: IRequestHandler<LoginCommand,LoginResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordService  _passwordService;
    private readonly IMediator _mediator;
    private readonly IRefreshToken _refreshToken;

    public LoginHendler(ICustomerRepository customerRepository, IMapper mapper, IPasswordService passwordService, IMediator mediator, IRefreshToken refreshToken)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _passwordService = passwordService;
        _mediator = mediator;
        _refreshToken = refreshToken;
    }


    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetCustomerByEmailAsync(request.Request.Email);
        if (customer is null)
        {
            throw new ResourseNotFoundException("Customer not found");
        }

        var passwordCheck =await _passwordService.VerifyPassword(request.Request.Password, customer.PasswordHash);
        if (!passwordCheck)
        {
            throw new BusinessException("Invalid email or password");
        }

        var accesToken = await _mediator.Send(new GenerateTokenCommand(customer));
        var refreshToken =
            new RefreshToken
            {
                CustomerId = customer.Id,


                Token =await _mediator.Send(new GenereteRefreshTokenCommand()),
                    


               ExpiresAt = DateTime.UtcNow
                    .AddDays(7),


                CreatedAt = DateTime.UtcNow
            };
        await _refreshToken.CreateAsync(refreshToken);
        await _refreshToken.SaveChangesAsync();
        return new LoginResponse()
        {
            AccessToken = accesToken,
            RefreshToken = refreshToken.Token
        };
    }
}