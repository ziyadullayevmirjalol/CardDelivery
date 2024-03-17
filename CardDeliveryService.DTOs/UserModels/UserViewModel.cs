using CardDeliveryService.Domain.Entities;

namespace CardDeliveryService.DTOs.UserModels;

public class UserViewModel
{
    public long Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public double Balance { get; set; }
    public List<Card>? Cards { get; set; }
}