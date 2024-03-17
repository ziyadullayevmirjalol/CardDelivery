using CardDeliveryService.Services.Interfaces;

namespace CardDeliveryService.Services.Services.AdminService;

public class AdminService : IAdminService
{
    public async ValueTask<bool> Login(string loginkey, string password)
    {
        if (loginkey != "sudologin")
            throw new Exception("loginkey is not correct");
        if (password != "sudopassword")
            throw new Exception("Password is not correct");

        return true;
    }
}
