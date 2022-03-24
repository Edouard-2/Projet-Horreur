using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIActivation))]
public class UILuminositySettings : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Luminosity Game");
    }
}
