using CardDeliveryService.Domain.Commons;

namespace CardDeliveryService.Domain.Entities;

public class User : Auditable
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public double Balance { get; set; }
    public List<Card>? Cards { get; set; }
    public string? UserCards { get; set; }
}
