using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Cart.AuthService.Command;

public record RegisterCommand(CustomerRequest Request) : IRequest<CustomerResponse>;
public class RegisterHendler:IRequestHandler<RegisterCommand,CustomerResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordService  _passwordService;

    public RegisterHendler(ICustomerRepository customerRepository, IMapper mapper, IPasswordService passwordService)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _passwordService = passwordService;
    }

    public async Task<CustomerResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingCustomer=await _customerRepository.GetCustomerByEmailAsync(request.Request.Email);
        if (existingCustomer is not null)
        {
            throw new ResourseIsAlredyExistException("Customer already exist");
        }
        var passwordHash= await _passwordService.HashPasword(request.Request.Password);
        var newCustomer=_mapper.Map<Models.Domain.Customer>(request.Request);
        newCustomer.PasswordHash = passwordHash;
        await _customerRepository.CreateAsync(newCustomer);
        await _customerRepository.SaveChangesAsync();
        return _mapper.Map<CustomerResponse>(newCustomer);
        
    }
}