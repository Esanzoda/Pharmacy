using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Customer.Command;

public record UpdateCustomerCommand(long Id, CustomerRequest Request) : IRequest<CustomerResponse>;

public class UpdateHendler : CustomerDiBase, IRequestHandler<UpdateCustomerCommand, CustomerResponse>
{
    public UpdateHendler(ICustomerRepository customerRepository, IMapper mapper)
        : base(customerRepository, mapper)
    {
    }

    public async Task<CustomerResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await CustomerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            throw new ResourseNotFoundException($"Customer not found with id {request.Id}");
        }

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

        Mapper.Map(request, customer);
        await CustomerRepository.UpdateAsync(customer);
        await CustomerRepository.SaveChangesAsync();
        return Mapper.Map<CustomerResponse>(customer);
    }
}