namespace Pharmasy.Models.Dto.Response;

public class  CartResponse
{
   public long Id { get; set; }
   public decimal TotalAmout { get; set; }
   public List<CartItemResponse> CartItemResponses { get; set; }
}
public class CartItemResponse
{
   public long CartId { get; set; }
   public long ProductId { get; set; }
   public int Quantity { get; set; }
   public decimal Price { get; set; }
   public decimal TotalAmout { get; set; }
}