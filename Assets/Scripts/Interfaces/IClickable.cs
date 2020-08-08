using UnityEngine.EventSystems;

namespace SweetAndSaltyStudios
{
    public interface IClickable
    {
        void OnPointerDown();
        void OnPointerClick();
        void OnPointerUp();

        void OnDoubleClick();
    }
}