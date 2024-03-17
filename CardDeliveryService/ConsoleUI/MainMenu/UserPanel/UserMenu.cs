using CardDeliveryService.Domain.Entities;
using CardDeliveryService.Presentation.ConsoleUI.MainMenu.UserPanel.UserAcitions;
using CardDeliveryService.Services.Services.CardService;
using CardDeliveryService.Services.Services.UserService;
using Spectre.Console;

namespace CardDeliveryService.Presentation.ConsoleUI.MainMenu.UserPanel;

public class UserMenu
{
    private User user;

    private UserService userService;
    private CardService cardService;

    private User_Actions useractions;

    public UserMenu(User user, UserService userService, CardService cardService)
    {
        this.user = user;
        this.userService = userService;
        this.cardService = cardService;

        useractions = new User_Actions(user, userService, cardService);
    }

    public async Task MenuAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choise = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Card[green]Delivery[/][red]/[/]User")
                    .PageSize(4)
                    .AddChoices(new[] {
                        "Deposit",
                        "Order New Card",
                        "View Ordered Cards",
                        "Remove Ordered Card",
                        "View Profile",
                        "Update User Details",
                        "Delete Account\n",
                        "[red]Sign out[/]"}));

            switch (choise)
            {
                case "Deposit":
                    AnsiConsole.Clear();
                    await useractions.Deposit();
                    break;
                case "Order New Card":
                    AnsiConsole.Clear();
                    await useractions.OrderNewCard();
                    break;
                case "View Ordered Cards":
                    AnsiConsole.Clear();
                    await useractions.ViewOrderedCards();
                    break;
                case "Remove Ordered Card":
                    AnsiConsole.Clear();
                    await useractions.RemoveOrderedCard();
                    break;
                case "View Profile":
                    AnsiConsole.Clear();
                    await useractions.ViewProfile();
                    break;
                case "Update User Details":
                    AnsiConsole.Clear();
                    await useractions.UpdateUserDetails();
                    break;
                case "Delete Account\n":
                    AnsiConsole.Clear();
                    await useractions.DeleteAccount();
                    return;
                case "[red]Sign out[/]":
                    return;
            }
        }
    }
}
