using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIActivation))]
public class UISetActiveOptions : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Open  Game");
    }
}
