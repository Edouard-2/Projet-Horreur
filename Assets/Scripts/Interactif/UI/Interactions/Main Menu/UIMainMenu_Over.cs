using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMainMenu_Over : MonoBehaviour
{
    private Vector3 m_scale;
    
    [SerializeField, Tooltip("Le textMeshPro de l'objet")]
    private TextMeshPro m_textMeshPro;
    
    [SerializeField, Tooltip("List de tout les waitForSeconds pour le glitch")]
    private List<WaitForSeconds> m_listWaitForSecond;

    private int m_randomIndex;
    
    private string m_text;
    
    private bool m_readyEnumerator;
    
    private void Awake()
    {
        m_scale = gameObject.transform.localScale;

        if (m_textMeshPro == null)
        {
            m_textMeshPro = GetComponent<TextMeshPro>();
            if (m_textMeshPro == null)
            {
                Debug.Log("Il faut mettre le textMeshPro !!");
            }
            m_text = "Error 404";
        }
        else
        {
            m_text = m_textMeshPro.text;
        }
    }

    private void OnMouseOver()
    {
        gameObject.transform.localScale = m_scale * 2;

        if (m_readyEnumerator)
        {
            m_readyEnumerator = false;
            m_randomIndex = Random.Range(2, m_listWaitForSecond.Count);
            StartCoroutine(ActiveGlitchText());
        }
    }

    IEnumerator ActiveGlitchText()
    {
        yield return m_listWaitForSecond[m_randomIndex];
        m_readyEnumerator = true;
    }

    private void OnMouseExit()
     {
         gameObject.transform.localScale = Vector3.one;
     }
 }