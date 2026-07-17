using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Deliver.Command;

public record UpdateDeliverComman(long Id, DeliverRequest Request) : IRequest<DeliverResponse>;

public class UpdateDeliverHendler : DeliverDiBase, IRequestHandler<UpdateDeliverComman, DeliverResponse>
{
    public UpdateDeliverHendler(IDeliverRepository deliverRepository, IMapper mapper, IDistributedCache cache)
        : base(deliverRepository, mapper, cache)
    {
    }

    public async Task<DeliverResponse> Handle(UpdateDeliverComman request, CancellationToken cancellationToken)
    {
        var deliver = await DeliverRepository.GetByIdAsync(request.Id);
        if (deliver is null)
        {
            throw new ResourseNotFoundException("Deliver not found");
        }


        if (await DeliverRepository.GetDeliverByEmail(request.Request.Email) != null)
        {
            throw new BusinessException("Deliver withthhis email alredy Exsist");
        }

        if (await DeliverRepository.GetDeliverByPhoneAsync(request.Request.PhoneNumber) != null)
        {
            throw new BusinessException("Deliver with this number alredy exsist");
        }

        Mapper.Map(request.Request, deliver);
        await DeliverRepository.UpdateAsync(deliver);
        await DeliverRepository.SaveChangesAsync();
        return Mapper.Map<DeliverResponse>(request.Request);
    }
}