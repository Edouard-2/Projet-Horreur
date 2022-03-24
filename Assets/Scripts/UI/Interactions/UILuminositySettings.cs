using UnityEngine;
using UnityEngine.EventSystems;

public class UILuminositySettings : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Luminosity Game");
    }
}
