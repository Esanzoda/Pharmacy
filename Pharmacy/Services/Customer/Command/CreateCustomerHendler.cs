using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Command;

public record CreateCustomerCommand(CustomerRequest Request) : IRequest<CustomerResponse>;

public class CreateCustomerHendler : CustomerDiBase, IRequestHandler<CreateCustomerCommand, CustomerResponse>
{
    private readonly ICartRepository _cartRepository;
    public CreateCustomerHendler(ICustomerRepository customerRepository, IMapper mapper, ICartRepository cartRepository)
        : base(customerRepository, mapper)
    {
        _cartRepository = cartRepository;
    }

    public async Task<CustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerByEmail = await CustomerRepository.GetCustomerByEmailAsync(request.Request.Email);
        if (customerByEmail != null)
        {
            throw new ResourseIsAlredyExistException(
                $"Customer already exists with this email {request.Request.Email}");
        }

        var customerByPhone = await CustomerRepository.GetCustomerByPhoneAsync(request.Request.PhoneNumber);
        if (customerByPhone != null)
        {
            throw new ResourseIsAlredyExistException(
                $"Customer already exists with this phone number{request.Request.PhoneNumber}");
        }

        var newCustomer = Mapper.Map<Models.Domain.Customer>(request.Request);
        await CustomerRepository.CreateAsync(newCustomer);
        await CustomerRepository.SaveChangesAsync();
        var cart = new Models.Domain.Cart
        {
            CustomerId = newCustomer.Id,
            Customer = newCustomer,
            TotalAmount = 0
        };
        
        await _cartRepository.CreateAsync(cart);
        await _cartRepository.SaveChangesAsync();
        return Mapper.Map<CustomerResponse>(newCustomer);
    }
}