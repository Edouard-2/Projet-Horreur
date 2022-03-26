using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(UIActivation))]
public class UISetActiveOptions : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Tooltip("Le panel d'UI")]
    private GameObject m_optionPanel;

    [SerializeField, Tooltip("L'UI Présent en même temps que le bouton OPTIONS (lui même y compris)")]
    private List<GameObject> m_listUIFront;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Open  Game");

        if (m_optionPanel.activeSelf)
        {
            HideOrDispalyOtherUI();
            m_optionPanel.SetActive(false);
            return;
        }

        HideOrDispalyOtherUI();
        m_optionPanel.SetActive(true);
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