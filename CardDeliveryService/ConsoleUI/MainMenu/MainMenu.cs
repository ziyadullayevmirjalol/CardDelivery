using CardDeliveryService.DataAccess.Contexts;
using CardDeliveryService.DataAccess.Repositores;
using CardDeliveryService.Domain.Entities;
using CardDeliveryService.Presentation.ConsoleUI.MainMenu.AdminPanel;
using CardDeliveryService.Presentation.ConsoleUI.MainMenu.UserPanel.UserAuthentification;
using CardDeliveryService.Services.Services.AdminService;
using CardDeliveryService.Services.Services.CardService;
using CardDeliveryService.Services.Services.UserService;
using Spectre.Console;

namespace CardDeliveryService.Presentation.ConsoleUI.MainMenu;

public class MainMenu
{
    private AppDbContext context;
    private Repository<User> Userrepository;
    private Repository<Card> Cardrepository;

    private AdminLogin adminLogin;
    private UserLogin userLogin;
    private UserRegister userRegister;

    private UserService userService;
    private CardService cardService;
    private AdminService adminService;

    public MainMenu()
    {
        context = new AppDbContext();

        Userrepository = new Repository<User>(context, context.Users);

        Cardrepository = new Repository<Card>(context, context.Cards);

        adminService = new AdminService();
        userService = new UserService(Userrepository, Cardrepository);
        cardService = new CardService(Cardrepository);

        adminLogin = new AdminLogin(adminService, cardService, userService);

        userLogin = new UserLogin(userService, cardService);
        userRegister = new UserRegister(userService, cardService);
    }

    #region Run
    public async Task RunAsync()
    {
        while (true)
        {
            var choise = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Card[green]Delivery[/]")
                    .PageSize(4)
                    .AddChoices(new[] {
                        "As User",
                        "As Administrator\n",
                        "[red]Exit[/]"}));

            switch (choise)
            {
                case "As User":
                    AnsiConsole.Clear();
                    await UserAskAsync();
                    break;
                case "As Administrator\n":
                    AnsiConsole.Clear();
                    await AdminAskAsync();
                    break;
                case "[red]Exit[/]":
                    return;
            }
        }
    }
    #endregion

    #region Customer
    public async Task UserAskAsync()
    {
        while (true)
        {
            var c = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("As User")
                .PageSize(4)
                .AddChoices(new[] {
                            "Login",
                            "Register\n",
                            "[red]Go Back[/]"}));
            switch (c)
            {
                case "Login":
                    await userLogin.LoginAsync();
                    break;
                case "Register\n":
                    await userRegister.RegisterAsync();
                    break;
                case "[red]Go Back[/]":
                    return;
            }
        }
    }
    #endregion

    #region Admin
    public async Task AdminAskAsync()
    {
        while (true)
        {
            var c = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("As Administrator")
                .PageSize(4)
                .AddChoices(new[] {
                            "Login\n",
                            "[red]Go Back[/]"}));
            switch (c)
            {
                case "Login\n":
                    await adminLogin.LoginAsync();
                    break;
                case "[red]Go Back[/]":
                    return;
            }
        }
    }
    #endregion
}
