using UnityEngine;
using UnityEngine.EventSystems;

public class TapZone : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Skyscraper.Instance.Tap();
    }
}
