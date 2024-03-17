using CardDeliveryService.DTOs.UserModels;
using CardDeliveryService.Services.Helpers;
using CardDeliveryService.Services.Services.CardService;
using CardDeliveryService.Services.Services.UserService;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace CardDeliveryService.Presentation.ConsoleUI.MainMenu.UserPanel.UserAuthentification;

public class UserRegister
{
    private UserService userService;
    private CardService cardService;
    private UserMenu userMenu;
    public UserRegister(UserService userService, CardService cardService)
    {
        this.cardService = cardService;
        this.userService = userService;
    }

    #region Registration
    public async Task RegisterAsync()
    {
        AnsiConsole.Clear();
        while (true)
        {
            var userCreateModel = new UserCreateModel();

            string email = string.Empty;
            while (string.IsNullOrWhiteSpace(email = AnsiConsole.Ask<string>("Enter your [green]email[/]")) || !Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                Console.WriteLine("Invalid email address!");
            }
        reenterpassword:
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your [green]password[/]:")
                    .PromptStyle("yellow")
            .Secret());
            while (password.Length < 8)
            {
                AnsiConsole.WriteLine("Password's length must be at least 8 characters");
                goto reenterpassword;
            }

            string firstname = AnsiConsole.Ask<string>("Enter your [green]Firstname[/]");
            string lastname = AnsiConsole.Ask<string>("Enter your [green]Lastname[/]");

            var HashedPassword = PasswordHashing.Hashing(password);

            userCreateModel.Email = email;
            userCreateModel.Password = HashedPassword;
            userCreateModel.Firstname = firstname;
            userCreateModel.Lastname = lastname;

            try
            {
                var createdUser = await userService.CreateUser(userCreateModel);
                var getUser = await userService.GetToLogin(email, HashedPassword);

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