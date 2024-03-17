using CardDeliveryService.DataAccess.Repositores;
using CardDeliveryService.Domain.Entities;
using CardDeliveryService.DTOs.CardModels;
using CardDeliveryService.Services.Extensions;
using CardDeliveryService.Services.Interfaces;

namespace CardDeliveryService.Services.Services.CardService;

public class CardService : ICardService
{
    private Repository<Card> repository;
    public CardService(Repository<Card> repository)
    {
        this.repository = repository;
    }

    public async ValueTask<CardViewModel> CreateCard(CardCreateModel card)
    {
        var created = await repository.InsertAsync(MapperExtension.MapTo<Card>(card));
        await repository.SaveAsync();
        return MapperExtension.MapTo<CardViewModel>(created);
    }

    public async ValueTask<bool> DeleteCard(long id)
    {
        var existCard = await repository.SelectByIdAsync(id)
            ?? throw new Exception("card is not found");

        await repository.DeleteAsync(existCard);
        await repository.SaveAsync();
        return true;
    }

    public async ValueTask<List<Card>> GetAllCards()
    {
        return (repository.SelectAllAsEnumerable().ToList());
    }

    public async ValueTask<Card> GetCard(long id)
    {
        var existCard = await repository.SelectByIdAsync(id)
            ?? throw new Exception("card is not found");
        return existCard;
    }

    public async ValueTask<CardViewModel> UpdateCard(long id, CardUpdateModel card)
    {
        var existCard = await repository.SelectByIdAsync(id)
            ?? throw new Exception("card is not found");

        existCard.bank = card.bank;
        existCard.CardName = card.CardName;
        existCard.CardNumber = card.CardNumber;


        var updatedCard = await repository.UpdateAsync(existCard);
        await repository.SaveAsync();
        return updatedCard.MapTo<CardViewModel>();
    }

    public async ValueTask<CardViewModel> ViewCard(long id)
    {
        var existCard = await repository.SelectByIdAsync(id)
            ?? throw new Exception("card is not found");
        return MapperExtension.MapTo<CardViewModel>(existCard);
    }
}
