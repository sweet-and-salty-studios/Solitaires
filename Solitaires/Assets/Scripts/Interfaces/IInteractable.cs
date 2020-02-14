using UnityEngine.EventSystems;

namespace SweetAndSaltyStudios
{
    public interface IInteractable : 
        IPointerDownHandler,
        IPointerClickHandler,
        IPointerUpHandler
    {

    }
}