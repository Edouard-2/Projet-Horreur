using TMPro;
using UnityEngine;

public class VisibleInvisibleTimer : MonoBehaviour
{
    [SerializeField, Tooltip("Script sur le timer ? checked: true")] bool m_isTimer;
    [SerializeField, Tooltip("Checked: Visible to Invisible")] bool m_isVisibleInvisible;
    
    private TextMeshPro m_test;
    [HideInInspector]public MeshRenderer m_render;
    private bool m_start;

    private void Awake()
    {
        if(m_isTimer) m_test = GetComponent<TextMeshPro>();
        else m_render = GetComponent<MeshRenderer>();

        DoVisibleToInvisible(false);
    }

    private void OnEnable()
    {
        PlayerManager.Instance.DoVisibleToInvisibleHandler += DoVisibleToInvisible;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.DoVisibleToInvisibleHandler -= DoVisibleToInvisible;
    }

    void DoVisibleToInvisible(bool p_start = false)
    {
        if (p_start)
        {
            //Debug.Log("hye start");
            m_start = !m_start;
        }
        
        //Allé
        if (m_start)
        {
            //Debug.Log("Start");
            if (m_isVisibleInvisible)
            {
                Hide();
                m_start = false;
                return;
            }
            Display();
            m_start = false;
            return;
        }
        Debug.Log("VI");
        //Retour
        if (m_isVisibleInvisible)
        {
            Display();
            m_start = true;
            return;
        }
        
        Hide();
        m_start = true;
    }

    void Display()
    {
        //  Mettre ombre
        if (m_test)
        {
            m_test.enabled = true;
        }
        //  Mettre Collider
        if (m_render)
        {
            m_render.enabled = true;
        }
    }

    void Hide()
    {
        //  Enlever ombre
        if (m_test)
        {
            m_test.enabled = false;
        }

        //  Enlever Collider
        if (m_render)
        {
            m_render.enabled = false;
        }
    }
}