using UnityEngine;

namespace SweetAndSaltyStudios
{
    public interface IDraggable
    {
        void OnBeginDrag();
        void OnDrag(Vector3 deltaPosition);
        void OnEndDrag();
    }
}