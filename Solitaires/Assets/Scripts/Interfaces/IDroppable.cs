using UnityEngine.EventSystems;

namespace SweetAndSaltyStudios
{
    public interface IDroppable : IDropHandler
    {
        bool IsValidDrop(CardDisplay[] cards);
    }
}