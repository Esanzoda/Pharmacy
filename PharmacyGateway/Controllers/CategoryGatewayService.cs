namespace PharmacyGateway.Controllers;

public class CategoryGatewayService
{
    private readonly HttpClient _httpClient;

    public CategoryGatewayService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}