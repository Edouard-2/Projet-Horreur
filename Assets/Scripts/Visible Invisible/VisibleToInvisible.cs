using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(VisibleToInvisibleMaterial))]
public class VisibleToInvisible : MonoBehaviour
{

    [SerializeField, Tooltip("True : l'objet est Visible puis Invisible / False : l'objet est Invisible puis Visible")]
    private bool m_isVisibleToInvisible;
    
    [ Tooltip("Est ce que l'objet a besoin du renderer")]public bool m_needRenderer;
    [SerializeField, Tooltip("False : Mettre le MeshRenderer de l'objet")]private MeshRenderer m_meshRenderer;

    [ Tooltip("Est ce que l'objet a besoin du collider ?")]public bool m_needCollider;
    [SerializeField, Tooltip("False : Mettre le collider de l'objet")]private BoxCollider m_boxCollider;

    private bool m_start = true;


    private void OnEnable()
    {
        PlayerManager.Instance.DoVisibleToInvisibleHandler += DoVisibleToInvisible;
    }

    private void Awake()
    {
        m_needRenderer = !m_needRenderer;
        m_needCollider = !m_needCollider;
        if(m_boxCollider == null && !m_needCollider)
        {
            m_boxCollider = GetComponent<BoxCollider>();
            
            if(m_boxCollider == null)
            {
                Debug.LogError("Remplit le collider Gros Chien !!!", this);
            }
        }
        
        if(m_meshRenderer == null && !m_needRenderer)
        {
            m_meshRenderer = GetComponent<MeshRenderer>();
            
            if(m_meshRenderer == null)
            {
                Debug.LogError("Remplit le Renderer Gros Chien !!!", this);
            }
        }
    }

    void DoVisibleToInvisible()
    {
        //Debug.Log("Function");
        //Allé
        if (m_start)
        {
            //Debug.Log("Start");
            if (m_isVisibleToInvisible)
            {
                Hide();
                m_start = false;
                return;
            }
            Display();
            m_start = false;
            return;
        }
        
        //.Log("None");
        //Retour
        if (m_isVisibleToInvisible)
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
        if (!m_needRenderer)
        {
            m_meshRenderer.shadowCastingMode = ShadowCastingMode.On;
        }
        //  Mettre Collider
        if (!m_needCollider)
        {
            m_boxCollider.enabled = true;
        }
    }

    void Hide()
    {
        //  Enlever ombre
        if (!m_needRenderer)
        {
            m_meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }

        //  Enlever Collider
        if (!m_needCollider)
        {
            m_boxCollider.enabled = false;
        }
    }
}
