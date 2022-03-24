using UnityEngine;
using UnityEngine.EventSystems;

public class UIResumeGame : MonoBehaviour, IPointerClickHandler
{
    private void OnEnable()
    {
        Debug.Log(UIManager.Instance.DoDisplayUIGamePause);
        UIManager.Instance.DoDisplayUIGamePause += SetVisible;
        Debug.Log(UIManager.Instance.DoDisplayUIGamePause);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Hey");
    }

    private void SetVisible(bool p_active = true)
    {
        gameObject.SetActive(p_active);
    }
}
