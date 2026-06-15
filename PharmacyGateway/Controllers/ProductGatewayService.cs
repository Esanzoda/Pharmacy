namespace PharmacyGateway.Controllers;

public class ProductGatewayService
{
    private readonly HttpClient _httpClient;

    public ProductGatewayService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}