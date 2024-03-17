using CardDeliveryService.Domain.Entities;
using CardDeliveryService.DTOs.CardModels;

namespace CardDeliveryService.Services.Interfaces;

public interface ICardService
{
    public ValueTask<CardViewModel> CreateCard(CardCreateModel card);
    public ValueTask<CardViewModel> UpdateCard(long id, CardUpdateModel card);
    public ValueTask<bool> DeleteCard(long id);
    public ValueTask<CardViewModel> ViewCard(long id);
    public ValueTask<Card> GetCard(long id);
    public ValueTask<List<Card>> GetAllCards();
}
