using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIActivation))]
public class UISetActiveOptions : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Tooltip("Le panel d'UI")]
    private GameObject m_optionPanel;

    [SerializeField, Tooltip("L'UI Présent en même temps que le bouton OPTIONS (lui même y compris)")]
    private List<GameObject> m_listUIFront;
    
    [SerializeField, Tooltip("Emitter du son Click")] private StudioEventEmitter m_clickEmitter;


    private void OnEnable()
    {
        GameManager.Instance.DoUiActivePauseGame += ActiveOrDesactive;
    }
    private void OnDisable()
    {
        GameManager.Instance.DoUiActivePauseGame -= ActiveOrDesactive;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Open  Game");
        m_clickEmitter.Play();
        ActiveOrDesactive(2);
    }

    private void ActiveOrDesactive(int p_i = 1)
    {
        if (p_i == 2)
        {
            Debug.Log("hey option escape");
            if (m_optionPanel.activeSelf)
            {
                UIManager.Instance.m_isOption = false;
                HideOrDispalyOtherUI();
                m_optionPanel.SetActive(false);
                return;
            }

            UIManager.Instance.m_isOption = true;
            HideOrDispalyOtherUI();
            m_optionPanel.SetActive(true);
        }
    }

    private void HideOrDispalyOtherUI()
    {
        if (m_listUIFront.Count != 0)
        {
            foreach (var UIelem in m_listUIFront)
            {
                if (UIelem.activeSelf)
                {
                    UIelem.SetActive(false);
                }
                else
                {
                    UIelem.SetActive(true);
                }
            }
        }
    }
}