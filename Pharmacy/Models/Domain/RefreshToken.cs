using Elastic.Clients.Elasticsearch.Security;

namespace Pharmasy.Models.Domain;

public class RefreshToken:BaseEntity
{
    public long CustomerId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public Customer Customer { get; set; } = null;
}