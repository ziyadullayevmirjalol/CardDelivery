namespace CardDeliveryService.Services.Interfaces;

public interface IAdminService
{
    public ValueTask<bool> Login(string loginkey, string password);
}
