using CardDeliveryService.Services.Helpers;
using CardDeliveryService.Services.Services.CardService;
using CardDeliveryService.Services.Services.UserService;
using Spectre.Console;

namespace CardDeliveryService.Presentation.ConsoleUI.MainMenu.UserPanel.UserAuthentification;

public class UserLogin
{
    private UserService userService;
    private CardService cardService;

    private UserMenu userMenu;

    public UserLogin(UserService userService, CardService cardService)
    {
        this.userService = userService;
        this.userService = userService;
    }

    #region Login
    public async Task LoginAsync()
    {
        AnsiConsole.Clear();
        while (true)
        {
            var email = AnsiConsole.Ask<string>("Enter your [green]email[/]:");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your [green]password[/]:")
                    .PromptStyle("yellow")
                    .Secret());

            try
            {
                var hashedpassword = PasswordHashing.Hashing(password);
                var getUser = await userService.GetToLogin(email, hashedpassword);

                userMenu = new UserMenu(getUser, userService, cardService);
                await userMenu.MenuAsync();
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
