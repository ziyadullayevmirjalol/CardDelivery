using CardDeliveryService.Presentation.ConsoleUI.MainMenu.AdminPanel.AdminActions;
using CardDeliveryService.Services.Services.AdminService;
using CardDeliveryService.Services.Services.CardService;
using CardDeliveryService.Services.Services.UserService;
using Spectre.Console;

namespace CardDeliveryService.Presentation.ConsoleUI.MainMenu.AdminPanel;

public class AdminMenu
{
    private UserService userService;
    private AdminService adminService;
    private CardService cardService;

    private Admin_Actions adminActions;
    public AdminMenu(UserService userService, AdminService adminService, CardService cardService)
    {
        this.adminService = adminService;
        this.userService = userService;
        this.cardService = cardService;

        this.adminActions = new Admin_Actions(adminService, userService, cardService);
    }

    public async Task MenuAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choise = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Card[green]Delivery[/][red]/[/]Admin")
                    .PageSize(4)
                    .AddChoices(new[] {
                 "Create New Card",
                 "Delete Card",
                 "Show All Users",
                 "Get All Cards",
                 "Get Card By ID\n",
                 //"Get Card by Filtering",
                 "[red]Sign out[/]"}));

            switch (choise)
            {
                case "Create New Card":
                    AnsiConsole.Clear();
                    await adminActions.CreateNewCard();
                    break;
                case "Delete Card":
                    AnsiConsole.Clear();
                    await adminActions.DeleteCard();
                    break;
                case "Show All Users":
                    AnsiConsole.Clear();
                    await adminActions.ShowAllUsers();
                    break;
                case "Get All Cards":
                    AnsiConsole.Clear();
                    await adminActions.GetAllCards();
                    break;
                case "Get Card By ID\n":
                    AnsiConsole.Clear();
                    await adminActions.GetCardById();
                    break;
                //case "Get Card by Filtering":
                //    AnsiConsole.Clear();
                //    await adminActions.GetCardByFiltering();
                //    break;
                case "[red]Sign out[/]":
                    return;
            }
        }
    }

}
