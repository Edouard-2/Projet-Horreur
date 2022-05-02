using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIActivation))]
public class UISwitchScene : MonoBehaviour, IPointerClickHandler
{
    [SerializeField,Tooltip("Index du niveau à charger")]private int m_levelIndex;
    [SerializeField,Tooltip("Monstre du niveau")]private GameObject m_monster;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_levelIndex == 0)
        {
            Destroy(m_monster);
            Destroy(PlayerManager.Instance.gameObject);
            Destroy(UIManager.Instance.gameObject);
            Destroy(GameManager.Instance.gameObject);
        }
        SceneManager.LoadSceneAsync(m_levelIndex);
    }
}