using CardDeliveryService.DTOs.CardModels;
using CardDeliveryService.Services.Services.AdminService;
using CardDeliveryService.Services.Services.CardService;
using CardDeliveryService.Services.Services.UserService;
using Spectre.Console;

namespace CardDeliveryService.Presentation.ConsoleUI.MainMenu.AdminPanel.AdminActions;

public class Admin_Actions
{
    private AdminService adminService;
    private UserService userService;
    private CardService cardService;

    public Admin_Actions(AdminService adminService, UserService userService, CardService cardService)
    {
        this.adminService = adminService;
        this.userService = userService;
        this.cardService = cardService;
    }
    public async Task CreateNewCard()
    {
        CardCreateModel card = new CardCreateModel();
        AnsiConsole.Markup("Available Banks \n" +
            "---------------\n" +
            "1. Qishloq Qurulish Bank\n" +
            "2. DavrBank\n" +
            "3. XalqBanki\n" +
            "4. AgroBank\n" +
            "5. SanoatQurulishBank\n" +
            "6. MikrokreditBank\n" +
            "7. TrastBank\n" +
            "8. Milly\n" +
            "9. Sberbank\n" +
            "---------------\n");
    again:
        var bank = AnsiConsole.Ask<int>("Choose [green]Bank[/]:");
        switch (bank)
        {
            case 1:
                card.bank = Domain.Enums.Bank.QishloqQurulishBank.ToString();
                break;
            case 2:
                card.bank = Domain.Enums.Bank.DavrBank.ToString();
                break;
            case 3:
                card.bank = Domain.Enums.Bank.XalqBanki.ToString();
                break;
            case 4:
                card.bank = Domain.Enums.Bank.Agrobank.ToString();
                break;
            case 5:
                card.bank = Domain.Enums.Bank.SanoatQurulishBank.ToString();
                break;
            case 6:
                card.bank = Domain.Enums.Bank.MikrokreditBank.ToString();
                break;
            case 7:
                card.bank = Domain.Enums.Bank.TrastBank.ToString();
                break;
            case 8:
                card.bank = Domain.Enums.Bank.Milliy.ToString();
                break;
            case 9:
                card.bank = Domain.Enums.Bank.Sberbank.ToString();
                break;
            default:
                AnsiConsole.MarkupLine("[red]Invalid choose[/] press any key to try again...");
                Console.ReadLine();
                goto again;
        }
        var CardName = AnsiConsole.Ask<string>("Enter [green]CardName[/]:");
        var CardNumber = AnsiConsole.Ask<int>("Enter [green]CardNumber[/]:");

        card.CardNumber = Convert.ToString(CardNumber);
        card.CardName = CardName;
        try
        {
            var created = await cardService.CreateCard(card);
            var table = new Table();

            table.AddColumn("[yellow]Created Card[/]");

            table.AddRow($"[green]Card ID[/]: {created.Id}");
            table.AddRow($"[green]From Bank[/]: {created.bank}");
            table.AddRow($"[green]Card name[/]: {created.CardName}");
            table.AddRow($"[green]Card number[/]: {created.CardNumber}");

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine("Press any key to exit...");
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
    }
    public async Task DeleteCard()
    {
        var id = AnsiConsole.Ask<long>("Enter [green]ID[/]:");
        try
        {
            await cardService.DeleteCard(id);

            AnsiConsole.WriteLine("[green]Card Deleted Successfully[/]\nPress any key to exit...");
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
    }
    public async Task ShowAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        if (users.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]there are no any users.[/]\nPress any key to exit...");
            Console.ReadLine();
        }
        else
        {
            foreach (var user in users)
            {
                var table = new Table();
                table.AddColumn("[yellow]User[/]");

                table.AddRow($"[green]Cusomer ID[/]: {user.Id}");
                table.AddRow($"[green]Email[/]: {user.Email}");
                table.AddRow($"[green]Firstname[/]: {user.Firstname}");
                table.AddRow($"[green]Lastname[/]: {user.Lastname}");

                AnsiConsole.Write(table);
            }
            AnsiConsole.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
    public async Task GetAllCards()
    {
        var cards = await cardService.GetAllCards();
        if (cards.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]there are no any cards.[/]\nPress any key to exit...");
            Console.ReadLine();
        }
        else
        {
            foreach (var card in cards)
            {
                var table = new Table();

                table.AddColumn("[yellow]Created Card[/]");

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
    public async Task GetCardById()
    {
        var id = AnsiConsole.Ask<long>("Enter [green]ID[/]:");
        try
        {
            var card = await cardService.GetCard(id);

            var table = new Table();

            table.AddColumn("[yellow]Created Card[/]");

            table.AddRow($"[green]Card ID[/]: {card.Id}");
            table.AddRow($"[green]From Bank[/]: {card.bank}");
            table.AddRow($"[green]Card name[/]: {card.CardName}");
            table.AddRow($"[green]Card number[/]: {card.CardNumber}");

            AnsiConsole.Write(table);

            AnsiConsole.WriteLine("Press any key to exit...");
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

    }
    //public async Task GetCardByFiltering()
    //{

    //}
}
