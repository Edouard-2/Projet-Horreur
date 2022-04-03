using UnityEngine;
using UnityEngine.EventSystems;

public class UIDeathScreen : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerManager.Instance.RestartLastSave();
    }
}
