using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMainMenu_Credit : MonoBehaviour
{
    [Header("Credit Settings")]
    [SerializeField, Tooltip("True : Si le joueur click il quittera le crédit")] 
    private bool m_isLeaveCredit;
    
    [SerializeField, Tooltip("True : Le credit monte (passe au texte en haut)")] 
    private bool m_isUpCredit;
    
    [SerializeField, Tooltip("True : Le credit Descend (passe au texte en haut)")] 
    private bool m_isDownCredit;
    
    [Range(1,30), SerializeField, Tooltip("True : Le credit monte (passe au texte en haut)")] 
    private int m_waitSwitchValue;
    
    [Range(1,30), SerializeField, Tooltip("True : Le credit monte (passe au texte en haut)")] 
    private int m_longWaitSwitchValue;
    
    [SerializeField, Tooltip("True : Le credit monte (passe au texte en haut)")] 
    private UIMainMenu_Credit m_creditParent;
    
    [Header("Credit Text")]
    [TextArea, SerializeField, Tooltip("Index de la scene pour commencer le jeu")] 
    private List<string> m_nomPrenomText;
    
    [TextArea,SerializeField, Tooltip("Index de la scene pour commencer le jeu")] 
    private List<string> m_matiereText;

    [Header("MainMenu")]
    [SerializeField, Tooltip("Index de la scene pour commencer le jeu")] 
    private GameObject m_mainMenu;
    
    [Header("TextMeshPro")]
    [SerializeField, Tooltip("Index de la scene pour commencer le jeu")] 
    private TextMeshPro m_textMeshProNomPrenom;
    
    [SerializeField, Tooltip("Index de la scene pour commencer le jeu")] 
    private TextMeshPro m_textMeshProMatiere;

    private WaitForSeconds m_waitSwitch;
    private WaitForSeconds m_longWaitSwitch;
    
    private int m_indexText = -1;

    private void OnEnable()
    {
        if (m_isUpCredit || m_isDownCredit || m_isLeaveCredit) return;
       
        m_indexText = -1;
        UpdateTextCredit(1);
        StartCoroutine(AutoSwitchText(m_waitSwitchValue));
    }

    private void Awake()
    {
        if (m_creditParent == null && (m_isUpCredit || m_isDownCredit))
        {
            transform.parent.GetComponent<UIMainMenu_Credit>();
            if (m_creditParent == null)
            {
                Debug.LogError("Gros Chien met le componetn !!!",this);
            }
        }
        m_waitSwitch = new WaitForSeconds(m_waitSwitchValue);
        m_waitSwitch = new WaitForSeconds(m_longWaitSwitchValue);
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("Appuye");
        
        if (m_isLeaveCredit)
        {
            transform.parent.gameObject.SetActive(false);
            m_mainMenu.SetActive(true);
            return;
        }
        
        if ( m_isDownCredit )
        {
            m_creditParent.ClickFunctionUpdate(1);
        }
        else if ( m_isUpCredit)
        {
            m_creditParent.ClickFunctionUpdate(-1);
        }
    }

    private void ClickFunctionUpdate(int p_dir)
    {
        
        if ( m_indexText > 0 && p_dir < 0 || m_matiereText.Count > m_indexText + 1 && p_dir > 0 )
        {
            StopAllCoroutines();
            
            StartCoroutine(AutoSwitchText(m_longWaitSwitchValue));
            
            UpdateTextCredit(p_dir);
        }
    }

    private void UpdateTextCredit(int p_upOrDown)
    {
        m_indexText += p_upOrDown;
        
        m_textMeshProMatiere.text = m_matiereText[m_indexText];
        m_textMeshProNomPrenom.text  = m_nomPrenomText[m_indexText];
    }

    IEnumerator AutoSwitchText(int p_wait)
    {
        yield return new WaitForSeconds(p_wait);
        
        if (m_matiereText.Count > m_indexText + 1 && m_nomPrenomText.Count > m_indexText + 1)
        {
            UpdateTextCredit(1);
            StartCoroutine(AutoSwitchText(m_waitSwitchValue));
        }
    }
}