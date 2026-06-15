namespace PharmacyGateway.Controllers;

public class CartGatewayService
{
    private readonly HttpClient _httpClient;

    public CartGatewayService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}