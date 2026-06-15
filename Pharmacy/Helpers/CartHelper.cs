
using Pharmasy.Models.Dto.Response;

namespace Pharmasy.Helpers;

public class CartHelper
{
public static class CartHelpers
{
    public static decimal GetTotal(List<CartItemResponse> cartItemDtos)
    {
        return cartItemDtos.Sum(item => item.TotalAmout);
    }
}
}