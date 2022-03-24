using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIActivation))]
public class UISoundSettings : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Sound Settings");
    }
}
