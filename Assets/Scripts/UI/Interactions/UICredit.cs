using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIActivation))]
public class UICredit : MonoBehaviour
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Crédits");
        Application.Quit();
    }
}
