namespace PharmacyGateway.Controllers;

public class EmployeGatewayServiice
{
    private readonly HttpClient _httpClient;

    public EmployeGatewayServiice(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}