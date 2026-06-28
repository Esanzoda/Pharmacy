using AutoMapper;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface ISupplierService : IBaseService<DeliverRequest, DeliverResponse>
{
}

public class  SupplierService : BaseService<Deliver, DeliverRequest, DeliverResponse>, ISupplierService
{
    public SupplierService(ISupplierRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}
    