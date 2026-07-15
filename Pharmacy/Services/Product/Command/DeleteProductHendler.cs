using MediatR;
using Pharmasy.Exception;
using Pharmasy.Repositories;

namespace Pharmasy.Services.Product.Command;

public record DeleteProductCommand(long Id): IRequest<bool>;
public class DeleteProductHendler:ProductDiBase,IRequestHandler<DeleteProductCommand,bool>
{
    public DeleteProductHendler(IProductRepository productRepository) : base(productRepository)
    {
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var category = await ProductRepository.DeleteAsync(request.Id);
        if (category is false)
        {
            throw new ResourseNotFoundException($"Product with id {request.Id} not found");
        }

        await ProductRepository.SaveChangesAsync();
        return category;
    }
}