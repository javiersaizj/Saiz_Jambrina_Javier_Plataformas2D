using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableUI : MonoBehaviour, IPointerClickHandler
{
    public System.Action OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
