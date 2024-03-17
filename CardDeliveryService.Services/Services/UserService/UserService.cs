using CardDeliveryService.DataAccess.Repositores;
using CardDeliveryService.Domain.Entities;
using CardDeliveryService.DTOs.UserModels;
using CardDeliveryService.Services.Extensions;
using CardDeliveryService.Services.Interfaces;

namespace CardDeliveryService.Services.Services.UserService;



public class UserService : IUserService
{
    private Repository<User> userRepository;
    private Repository<Card> cardRepository;

    public UserService(Repository<User> userrepository, Repository<Card> cardrepository)
    {
        this.userRepository = userrepository;
        this.cardRepository = cardrepository;
    }

    public async ValueTask<User> BookNewCard(long userId, long cardId)
    {
        var existUser = await userRepository.SelectByIdAsync(userId)
            ?? throw new Exception("User is not found");
        var existCard = cardRepository.SelectAllAsQueryable().FirstOrDefault(card => card.Id == cardId && (card.CardHolderId == null || card.CardHolderId == 0 && card.CardHolderId != existUser.Id))
            ?? throw new Exception("Card is not found");

        if (existUser.Cards == null)
        {
            existUser.Cards = new List<Card>();
            existUser.Cards.Add(existCard);
            existCard.CardHolderId = existUser.Id;
        }
        else
        {
            existUser.Cards.Add(existCard);
            existCard.CardHolderId = existUser.Id;
        }
        await userRepository.SaveAsync();
        await cardRepository.SaveAsync();
        return existUser;
    }
    public async ValueTask<User> CreateUser(UserCreateModel user)
    {
        var createdUser = await userRepository.InsertAsync(user.MapTo<User>());

        await userRepository.SaveAsync();
        return createdUser;
    }
    public async ValueTask<bool> DeleteUser(long id)
    {
        var existUser = await userRepository.SelectByIdAsync(id)
            ?? throw new Exception("User is not found");

        await userRepository.DeleteAsync(existUser);
        await userRepository.SaveAsync();
        return true;
    }
    public async ValueTask<User> DepositAsync(long userId, double amount)
    {
        var existUser = await userRepository.SelectByIdAsync(userId)
            ?? throw new Exception($"User is not exists with Id: {userId}");
        existUser.Balance += amount;
        await userRepository.SaveAsync();
        return existUser;
    }
    public async ValueTask<List<Card>> GetAllBookedCards(long userId)
    {
        var existUser = await userRepository.SelectByIdAsync(userId)
            ?? throw new Exception($"User is not exists with Id: {userId}");

        if (existUser.Cards == null || existUser.Cards.Count == 0)
            throw new Exception("User does not have any cards");

        return existUser.Cards;
    }
    public async ValueTask<List<User>> GetAllUsersAsync()
    {
        return userRepository.SelectAllAsEnumerable().ToList();
    }
    public async ValueTask<User> GetToLogin(string email, string password)
    {
        var existUser = userRepository.SelectAllAsQueryable().FirstOrDefault(user => user.Email == email && user.Password == password && !user.IsDeleted)
            ?? throw new Exception("User is not found");

        return existUser;
    }
    public async ValueTask<User> GetUser(long id)
    {
        return await userRepository.SelectByIdAsync(id);
    }
    public async ValueTask<User> RemoveBookedCard(long userId, long cardId)
    {
        var existUser = await userRepository.SelectByIdAsync(userId)
            ?? throw new Exception("User is not found");


        if (existUser.Cards == null)
            throw new Exception("User does not have any cars");
        else
        {
            var existCard = existUser.Cards.FirstOrDefault(card => card.Id == cardId)
                ?? throw new Exception("card is not found on user's card list");

            var existCardDB = cardRepository
                .SelectAllAsQueryable()
                .FirstOrDefault(card => card.Id == existCard.Id && card.CardHolderId == existUser.Id)
                       ?? throw new Exception("Card is not found on database");

            existUser.Cards.Remove(existCard);
            existCardDB.CardHolder = null;

            await userRepository.SaveAsync();
            await cardRepository.SaveAsync();
        }
        return existUser;
    }
    public async ValueTask<UserViewModel> UpdateUser(long id, UserUpdateModel user)
    {
        var existUser = await userRepository.SelectByIdAsync(id)
            ?? throw new Exception("User is not found");

        existUser.Email = user.Email;
        existUser.Password = user.Password;
        existUser.Firstname = user.Firstname;
        existUser.Lastname = user.Lastname;

        var updatedUser = await userRepository.UpdateAsync(existUser);
        await userRepository.SaveAsync();
        return updatedUser.MapTo<UserViewModel>();
    }
    public async ValueTask<UserViewModel> ViewUser(long id)
    {
        var existUser = await userRepository.SelectByIdAsync(id)
            ?? throw new Exception("User is not found");

        return existUser.MapTo<UserViewModel>();
    }
}
