using CardDeliveryService.Domain.Entities;
using CardDeliveryService.DTOs.UserModels;
using CardDeliveryService.Services.Helpers;
using CardDeliveryService.Services.Services.CardService;
using CardDeliveryService.Services.Services.UserService;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace CardDeliveryService.Presentation.ConsoleUI.MainMenu.UserPanel.UserAcitions;

public class User_Actions
{
    private User user;
    private UserService userService;
    private CardService cardService;

    public User_Actions(User user, UserService userService, CardService cardService)
    {
        this.user = user;
        this.userService = userService;
        this.cardService = cardService;
    }

    public async Task Deposit()
    {
        var amount = AnsiConsole.Ask<double>("Enter [green]amount[/]: ");
        await AnsiConsole.Status()
     .Start("Process...", async ctx =>
     {
         AnsiConsole.MarkupLine("loading services...");
         try
         {
             user = await userService.DepositAsync(user.Id, amount);
         }
         catch (Exception ex)
         {
             AnsiConsole.WriteLine(ex.Message);
         }
         Thread.Sleep(1000);
         AnsiConsole.Clear();
     });
        AnsiConsole.MarkupLine("[green]Done[/] Press any key to continue...");
        Console.ReadLine();
    }
    public async Task OrderNewCard()
    {

        var cardId = AnsiConsole.Ask<int>("Enter [green]Card ID[/]: ");
        await AnsiConsole.Status()
     .Start("Process...", async ctx =>
     {
         AnsiConsole.MarkupLine("loading services...");
         try
         {
             AnsiConsole.Clear();
             user = await userService.BookNewCard(user.Id, cardId);
             AnsiConsole.MarkupLine("[green]You Booked a new card[/] Press any key to continue...");
             Console.ReadLine();
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

         AnsiConsole.Clear();
     });
    }
    public async Task ViewOrderedCards()
    {
        user = await userService.GetUser(user.Id);
        if (user.Cards.Count == 0 || user.Cards == null)
        {
            AnsiConsole.MarkupLine("[yellow]there are no any cards.[/]\nPress any key to exit...");
            Console.ReadLine();
        }
        else
        {
            foreach (var card in user.Cards)
            {
                var table = new Table();

                table.AddColumn("[yellow]Card[/]");

                table.AddRow($"[green]Card ID[/]: {card.Id}");
                table.AddRow($"[green]From Bank[/]: {card.bank}");
                table.AddRow($"[green]Card name[/]: {card.CardName}");
                table.AddRow($"[green]Card number[/]: {card.CardNumber}");

                AnsiConsole.Write(table);
            }
            AnsiConsole.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
    public async Task RemoveOrderedCard()
    {
        var cardId = AnsiConsole.Ask<int>("Enter [green]Card ID[/]: ");
        await AnsiConsole.Status()
     .Start("Process...", async ctx =>
     {
         AnsiConsole.MarkupLine("loading services...");
         try
         {
             AnsiConsole.Clear();
             user = await userService.RemoveBookedCard(user.Id, cardId);
             AnsiConsole.MarkupLine("[green]You removed a booked card[/] Press any key to continue...");
             Console.ReadLine();
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

         AnsiConsole.Clear();
     });
    }
    public async Task ViewProfile()
    {
        var userView = await userService.ViewUser(user.Id);

        var table = new Table();
        if (userView.Cards != null || userView.Cards.Count == 0)
        {
            table.AddColumn("[yellow]Your Profile[/]");

            table.AddRow($"[green]User ID[/]: {userView.Id}");
            table.AddRow($"[green]Email[/]: {userView.Email}");
            table.AddRow($"[green]Balance ($)[/]: {userView.Balance}");
            table.AddRow($"[green]Firstname[/]: {userView.Firstname}");
            table.AddRow($"[green]Lastname[/]: {userView.Lastname}");
            table.AddRow($"[green]Cards[/]: {userView.Cards.Count}");

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
        else
        {
            table.AddColumn("[yellow]Your Profile[/]");

            table.AddRow($"[green]User ID[/]: {userView.Id}");
            table.AddRow($"[green]Email[/]: {userView.Email}");
            table.AddRow($"[green]Balance ($)[/]: {userView.Balance}");
            table.AddRow($"[green]Firstname[/]: {userView.Firstname}");
            table.AddRow($"[green]Lastname[/]: {userView.Lastname}");
            table.AddRow($"[green]Cards[/]: {"Does not have any cards"}");

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
    public async Task UpdateUserDetails()
    {
        UserUpdateModel userUpdate = new UserUpdateModel();

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
        string email = string.Empty;
        while (string.IsNullOrWhiteSpace(email = AnsiConsole.Ask<string>("Enter your [green]email[/]")) || !Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        {
            Console.WriteLine("Invalid email address!");
        }
        string firstname = AnsiConsole.Ask<string>("Enter your [green]Firstname[/]");
        string lastname = AnsiConsole.Ask<string>("Enter your [green]Lastname[/]");
        var hashedpassword = PasswordHashing.Hashing(password);

        userUpdate.Password = hashedpassword;
        userUpdate.Email = email;
        userUpdate.Firstname = firstname;
        userUpdate.Lastname = lastname;

        try
        {
            await userService.UpdateUser(user.Id, userUpdate);
            user = await userService.GetUser(user.Id);
            AnsiConsole.MarkupLine("[green]Success[/] Press any key to continue...");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.Clear();
            Console.WriteLine(ex.Message);
            AnsiConsole.WriteLine("Press any key to exit...");
            Console.ReadLine();
            AnsiConsole.Clear();
            return;
        }
    }
    public async Task DeleteAccount()
    {

    reenter:
        AnsiConsole.WriteLine($"Are you sure you want to delete your account with email: {user.Email}?...");
        AnsiConsole.Write("Press (yes) to confirm, (no) to cancel:");
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "yes":
                try
                {
                    AnsiConsole.Clear();
                    await userService.DeleteUser(user.Id);
                    AnsiConsole.MarkupLine("[green]Success[/]Press any key to exit...");
                    Console.ReadLine();
                    AnsiConsole.Clear();
                    return;
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                    AnsiConsole.WriteLine("Press any key to exit...");
                    Console.ReadLine();
                    AnsiConsole.Clear();
                    return;
                }
            case "no":
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[green]Canceled[/]");
                Thread.Sleep(1000);
                AnsiConsole.Clear();
                return;
            default:
                Console.WriteLine("invalid input");
                await Console.Out.WriteLineAsync("Press any key to reenter...");
                Console.ReadLine();
                goto reenter;
        }
    }
}
