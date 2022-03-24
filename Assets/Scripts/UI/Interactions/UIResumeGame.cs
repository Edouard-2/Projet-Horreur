using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIActivation))]
public class UIResumeGame : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Resume Game");
        GameManager.Instance.SwitchPauseGame();
    }
}
