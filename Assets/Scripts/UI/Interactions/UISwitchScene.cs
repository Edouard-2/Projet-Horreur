using UnityEngine;
using UnityEngine.EventSystems;

public class UISwitchScene : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Main Menu");
    }
}
