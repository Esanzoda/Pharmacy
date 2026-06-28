namespace Pharmasy.Models.Domain;

public class Cart:BaseEntity
{
   public long UserId { get; set; }
   public User? User { get; set; }
   public decimal TotalAmout { get; set; }

   public List<CartItem> CartItems { get; set; }
      = new List<CartItem>();

}  