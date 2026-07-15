using AutoMapper;
using MediatR;
using Pharmasy.Exception;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Deliver.Command;

public record CreateDeliverCommand(DeliverRequest DeliverRequest) : IRequest<DeliverResponse>;

public class CreateDeliverHendler : IRequestHandler<CreateDeliverCommand, DeliverResponse>
{
    private readonly IDeliverRepository _deliverRepository;
    private readonly IMapper _mapper;

    public CreateDeliverHendler(IDeliverRepository deliverRepository, IMapper mapper)
    {
        _deliverRepository = deliverRepository;
        _mapper = mapper;
    }

    public async Task<DeliverResponse> Handle(CreateDeliverCommand request, CancellationToken cancellationToken)
    {
        var deliver = await _deliverRepository.GetDeliverByEmail(request.DeliverRequest.Email);
        if (deliver is not null)
        {
            throw new ResourseIsAlredyExistException("Deliver already exist");
        }

        var newDeliver = _mapper.Map<Models.Domain.Deliver>(request.DeliverRequest);
        await _deliverRepository.CreateAsync(newDeliver);
        await _deliverRepository.SaveChangesAsync();

        return _mapper.Map<DeliverResponse>(newDeliver);
    }
}