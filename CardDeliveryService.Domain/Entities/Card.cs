using CardDeliveryService.Domain.Commons;

namespace CardDeliveryService.Domain.Entities;

public class Card : Auditable
{
    public string bank { get; set; }
    public string CardName { get; set; }
    public string CardNumber { get; set; }
    public User? CardHolder { get; set; }
    public long? CardHolderId { get; set; }
}
