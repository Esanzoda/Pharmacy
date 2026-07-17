using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Deliver.Command;

public record CreateDeliverCommand(DeliverRequest DeliverRequest) : IRequest<DeliverResponse>;

public class CreateDeliverHendler : DeliverDiBase,IRequestHandler<CreateDeliverCommand, DeliverResponse>
{
    public CreateDeliverHendler(IDeliverRepository deliverRepository, IMapper mapper, IDistributedCache cache) 
        : base(deliverRepository, mapper, cache)
    {
    }

    public async Task<DeliverResponse> Handle(CreateDeliverCommand request, CancellationToken cancellationToken)
    {
        var deliver = await DeliverRepository.GetDeliverByEmail(request.DeliverRequest.Email);
        if (deliver is not null)
        {
            throw new ResourseIsAlredyExistException("Deliver already exist");
        }

        var newDeliver = Mapper.Map<Models.Domain.Deliver>(request.DeliverRequest);
        await DeliverRepository.CreateAsync(newDeliver);
        await DeliverRepository.SaveChangesAsync();

        return Mapper.Map<DeliverResponse>(newDeliver);
    }
}