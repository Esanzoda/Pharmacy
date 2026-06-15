using AutoMapper;
using Pharmasy.Models.Domain;
using Pharmasy.Models.Dto.Request;
using Pharmasy.Models.Dto.Response;
using Pharmasy.Repositories;

namespace Pharmasy.Services;

public interface ISupplierService : IBaseService<SupplierRequest, SupplierResponse>
{
    
}
public class SupplierService:BaseService<Supplier,SupplierRequest,SupplierResponse>, ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    public SupplierService(ISupplierRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
        _supplierRepository = repository;
    }
}