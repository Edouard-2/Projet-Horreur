using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIActivation))]
public class UISwitchScene : MonoBehaviour, IPointerClickHandler
{
    [SerializeField,Tooltip("Index du niveau à charger")]public int m_levelIndex;
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
        NextLevel();
    }

    public void NextLevel()
    {
        if (m_levelIndex == 0 || m_levelIndex == 2 )
        {
            PlayerManager.Instance.RemoveAllPostProcess();
            DestroyImmediate(m_monster1);
            DestroyImmediate(m_monster2);
            DestroyImmediate(PlayerManager.Instance.gameObject);
            DestroyImmediate(UIManager.Instance.gameObject);
            DestroyImmediate(GameManager.Instance.gameObject);
            Cursor.lockState = CursorLockMode.Confined;
        }

        SceneManager.LoadSceneAsync(m_levelIndex);
    }
}