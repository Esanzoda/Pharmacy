namespace PharmacyGateway.Controllers;

public class OrderGatewayService
{
    private readonly HttpClient _httpClient;
    public OrderGatewayService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}