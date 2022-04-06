using System.Collections;
using TMPro;
using UnityEngine;

public class UIMainMenu_Over : MonoBehaviour
{
    private Vector3 m_scale;

    [SerializeField, Tooltip("Le textMeshPro de l'objet")]
    private TextMeshPro m_textMeshPro;
    
    [SerializeField, Tooltip("Est ce que le text est un bouton")]
    private bool m_isButton;
    
    [SerializeField, Tooltip("Est ce que le text est glitcher")]
    private bool m_isGlitched = true;

    private WaitForSeconds m_waitForSecond = new WaitForSeconds(1f);

    private int m_randomIndex;

    private string m_initText;
    private string m_text = "Error 404";

    private string m_baseString =
        "&é'(-è_çà)=azertyuiop^$qsdfghjklmù*wxcvbn,;:!'1234567890°+£¨µ%§/ML?KIOPUJNBHYTGVCFREDXSWZQA<";

    private bool m_readyEnumerator = true;

    private void Awake()
    {
        m_scale = gameObject.transform.localScale;

        if (m_textMeshPro == null)
        {
            m_textMeshPro = GetComponent<TextMeshPro>();
            if (m_textMeshPro == null)
            {
                Debug.LogError("Il faut mettre le textMeshPro !!", this);
            }
        }
        else
        {
            m_text = m_textMeshPro.text;
            m_initText = m_text;
        }
    }

    private void OnMouseOver()
    {
        if (m_isButton)
        {
            gameObject.transform.localScale = m_scale * 1.1f;
        }

        if (m_readyEnumerator && m_isGlitched)
        {
            m_readyEnumerator = false;
            
            int chooseNumberLetter = Random.Range(1, m_text.Length);

            int[] tabIndexChoseLetter = new int[chooseNumberLetter];
            
            for (int i = 0; i < tabIndexChoseLetter.Length; i++)
            {
                int rand = Random.Range(0, m_text.Length - 1);
                
                if (tabIndexChoseLetter[i] != rand)
                {
                    tabIndexChoseLetter[i] = rand;
                }
            }
            
            tabIndexChoseLetter = TriArraye(tabIndexChoseLetter);

            string Test = "";

            for (int i = 0; i < m_text.Length; i++)
            {
                if ( tabIndexChoseLetter.Length > i && i == tabIndexChoseLetter[i] )
                {
                    
                    Test += m_baseString[Random.Range(0, m_baseString.Length)];
                }
                else
                {
                    Test += m_text[i];
                }
            }

            m_textMeshPro.SetText(Test);
            StartCoroutine(ActiveGlitchText());
        }
    }

    static int[] TriArraye(int[] p_tab)
    {
        int n = p_tab.Length - 1;
        for (int i = n; i >= 1; i--)
        {
            for (int j = 2; j <= i; j++)
            {
                if (p_tab[j - 1] > p_tab[j])
                {
                    (p_tab[j-1], p_tab[j]) = (p_tab[j], p_tab[j-1]);
                }
            }
        }
        return p_tab;
    }
    private void OnMouseExit()
    {
        if (m_isButton)
        {
            gameObject.transform.localScale = m_scale;
        }

        m_textMeshPro.SetText(m_initText);
        
        m_readyEnumerator = true;
    }

    IEnumerator ActiveGlitchText()
    {
        yield return m_waitForSecond;
        m_readyEnumerator = true;
    }
}