using Pharmasy.Models.Dto.Response;

namespace Pharmasy.Helpers;

public static class CartHelper
{
    public static decimal GetTotal(this List<CartItemResponse> cartItemDtos)
    {
        return cartItemDtos.Sum(item => item.TotalPrice);
    }
}