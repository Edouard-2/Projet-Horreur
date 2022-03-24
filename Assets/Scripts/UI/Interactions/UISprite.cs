using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISprite : MonoBehaviour
{
    [SerializeField, Tooltip("True: Ui au main menu / False: Ui pour le menu in-game (pause)")]private bool m_isMainMenu;
    
    private void OnEnable()
    {
        if (m_isMainMenu)
        {
            UIManager.Instance.DoDisplayUIMainMenu += UIActiveSelf;
            return;
        }
        UIManager.Instance.DoDisplayUIGamePause += UIActiveSelf;
    }
    private void OnDisable()
    {
        if (m_isMainMenu)
        {
            UIManager.Instance.DoDisplayUIMainMenu -= UIActiveSelf;
            return;
        }
        UIManager.Instance.DoDisplayUIGamePause -= UIActiveSelf;
    }

    private void UIActiveSelf(bool p_active)
    {
        //Faire l'affichage ou nn
        Debug.Log($"Ui.isActive = {p_active}");
    }
}
