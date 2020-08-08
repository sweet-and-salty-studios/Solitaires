using UnityEngine;

namespace SweetAndSaltyStudios
{
    public interface IHighlightResponse
    {
        RectTransform RectTransform { get; }

        void OnHoverIn();
        void OnHoverOut();
    }
}