using AutoMapper;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface ISupplierService : IBaseService<DeliverRequest, DeliverResponse>
{
}

public class DeliverService : BaseService<Deliver, DeliverRequest, DeliverResponse>, ISupplierService
{
    public DeliverService(IDeliverRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}