using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIActivation))]
public class UICredit : MonoBehaviour, IPointerClickHandler
{
    [SerializeField,Tooltip("Index du niveau à charger")]private int m_levelIndex;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadSceneAsync(m_levelIndex);
    }
}
