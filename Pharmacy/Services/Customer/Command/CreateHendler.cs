using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Command;

public record CreateCustomerCommand(CustomerRequest Request) : IRequest<CustomerResponse>;

public class CreateHendler : CustomerDiBase, IRequestHandler<CreateCustomerCommand, CustomerResponse>
{
    public CreateHendler(ICustomerRepository customerRepository, IMapper mapper)
        : base(customerRepository, mapper)
    {
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

        var newCustomer = Mapper.Map<Models.Domain.Customer>(request);
        await CustomerRepository.CreateAsync(newCustomer);
        await CustomerRepository.SaveChangesAsync();
        var cart = new Models.Domain.Cart
        {
            CustomerId = newCustomer.Id,
            Customer = newCustomer,
            TotalAmount = 0
        };
        return Mapper.Map<CustomerResponse>(newCustomer);
    }
}