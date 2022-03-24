using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIActivation))]
public class UISwitchScene : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Main Menu");
    }
}
