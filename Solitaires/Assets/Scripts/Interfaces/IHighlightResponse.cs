using UnityEngine;

namespace SweetAndSaltyStudios
{
    public interface IHighlightResponse
    {
        RectTransform RectTransform { get; }

        void HoverInAnimation();
        void HoverOutAnimation();
    }
}