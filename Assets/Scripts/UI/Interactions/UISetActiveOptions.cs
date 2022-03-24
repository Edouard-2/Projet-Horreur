using UnityEngine;
using UnityEngine.EventSystems;

public class UISetActiveOptions : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Open Game");
    }
}
