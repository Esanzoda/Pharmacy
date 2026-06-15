namespace PharmacyGateway.Controllers;

public class CustomerGatewayService
{
    private readonly HttpClient _httpClient;

    public CustomerGatewayService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}