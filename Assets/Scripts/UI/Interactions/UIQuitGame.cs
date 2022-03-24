using UnityEngine;
using UnityEngine.EventSystems;

public class UIQuitGame : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Quit Game");
    }
}
