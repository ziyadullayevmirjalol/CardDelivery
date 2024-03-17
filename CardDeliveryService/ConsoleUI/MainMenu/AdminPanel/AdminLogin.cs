using CardDeliveryService.Services.Services.AdminService;
using CardDeliveryService.Services.Services.CardService;
using CardDeliveryService.Services.Services.UserService;
using Spectre.Console;

namespace CardDeliveryService.Presentation.ConsoleUI.MainMenu.AdminPanel;

public class AdminLogin
{
    private AdminService adminService;
    private CardService cardService;
    private UserService userService;

    private AdminMenu adminMenu;

    public AdminLogin(AdminService adminService, CardService cardService, UserService userService)
    {
        this.userService = userService;
        this.adminService = adminService;
        this.cardService = cardService;
    }

    #region Login
    public async Task LoginAsync()
    {
        AnsiConsole.Clear();
        while (true)
        {
            var loginkey = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]loginkey[/]:")
            .PromptStyle("yellow")
            .Secret());
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]password[/]:")
            .PromptStyle("yellow")
            .Secret());

            try
            {
                var getAdmin = await adminService.Login(loginkey, password);

                adminMenu = new AdminMenu(userService, adminService, cardService);
                await adminMenu.MenuAsync();
                return;
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                AnsiConsole.WriteLine("Press any key to exit and try again.");
                Console.ReadLine();
                AnsiConsole.Clear();
                return;
            }
        }
    }
    #endregion
}
