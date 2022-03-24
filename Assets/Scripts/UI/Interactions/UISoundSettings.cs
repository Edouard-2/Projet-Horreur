using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundSettings : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Sound Settings");
    }
}
