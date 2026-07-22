using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeeByIdQuery(
    long Id
) : IRequest<EmployeeResponse>;

public class GetEmployeeByIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper,
    IDistributedCache cache
) : IRequestHandler<GetEmployeeByIdQuery, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var key = $"EmployeeById-{request.Id}";
        var cachedEmployee = await cache.GetStringAsync(key, cancellationToken);
        if (cachedEmployee is not null)
        {
            var redis = JsonConvert.DeserializeObject<EmployeeResponse>(cachedEmployee);

            if (redis is not null)
            {
                return redis;
            }
        }

        var employee = await dbContext.Employees
            .FindAsync(request.Id, cachedEmployee);

        if (employee == null)
        {
            throw new ResourseNotFoundException("customer not found");
        }

        var response = mapper.Map<EmployeeResponse>(employee);
        await cache.SetStringAsync(
            key,
            JsonConvert.SerializeObject(response),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            },
            cancellationToken);
        return response;
    }
}