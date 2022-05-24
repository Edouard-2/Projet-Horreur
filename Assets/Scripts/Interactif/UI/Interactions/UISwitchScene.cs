using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIActivation))]
public class UISwitchScene : MonoBehaviour, IPointerClickHandler
{
    [SerializeField,Tooltip("Index du niveau à charger")]private int m_levelIndex;
    [SerializeField,Tooltip("Monstre du niveau")]private GameObject m_monster1;
    [SerializeField,Tooltip("Monstre du niveau")]private GameObject m_monster2;
    [SerializeField, Tooltip("Emitter du son Click")] private StudioEventEmitter m_clickEmitter;

    private void Awake()
    {
        if (m_monster1 == null || m_monster2 == null)
        {
            Debug.LogError("Ya pas le monstre dans l'ui Main Menu");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_clickEmitter.Play();
        if (m_levelIndex == 0)
        {
            PlayerManager.Instance.RemoveAllPostProcess();
            Destroy(m_monster1);
            Destroy(m_monster2);
            Destroy(PlayerManager.Instance.gameObject);
            Destroy(UIManager.Instance.gameObject);
            Destroy(GameManager.Instance.gameObject);
        }
        SceneManager.LoadSceneAsync(m_levelIndex);
    }
}