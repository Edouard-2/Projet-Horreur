using UnityEngine;
using UnityEngine.Rendering;

public class VisibleToInvisible : MonoBehaviour
{

    [SerializeField, Tooltip("True : l'objet est Visible puis Invisible / False : l'objet est Invisible puis Visible")]
    private bool m_isVisibleToInvisible;
    
    [SerializeField, Tooltip("Mettre le MeshRenderer de l'objet")]private MeshRenderer m_meshRenderer;

    [SerializeField, Tooltip("Mettre le collider de l'objet")]private BoxCollider m_boxCollider;

    private bool m_start = true;

    private void Awake()
    {
        if(m_boxCollider == null)
        {
            m_boxCollider = GetComponent<BoxCollider>();
            
            if(m_boxCollider == null)
            {
                Debug.LogError("Remplit le collider Gros Chien !!!", this);
            }
        }
        
        if(m_meshRenderer == null)
        {
            m_meshRenderer = GetComponent<MeshRenderer>();
            
            if(m_meshRenderer == null)
            {
                Debug.LogError("Remplit le collider Gros Chien !!!", this);
            }
        }
    }

    private void OnEnable()
    {
        PlayerManager.Instance.DoVisibleToInvisibleHandler += DoVisibleToInvisible;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.DoVisibleToInvisibleHandler -= DoVisibleToInvisible;
    }

    void DoVisibleToInvisible()
    {
        Debug.Log("Function");
        //Allé
        if (m_start)
        {
            Debug.Log("Start");
            if (m_isVisibleToInvisible)
            {
                Hide();
                m_start = false;
                return;
            }
            m_start = false;
            Display();
            return;
        }
        
        Debug.Log("None");
        //Retour
        if (m_isVisibleToInvisible)
        {
            Display();
            m_start = true;
            return;
        }
        
        m_start = true;
        Hide();
    }

    void Display()
    {
        Debug.Log("Display");
        //  Mettre ombre
        m_meshRenderer.shadowCastingMode = ShadowCastingMode.On;
        //  Mettre Collider
        m_boxCollider.enabled = true;
    }

    void Hide()
    {
        Debug.Log("Hide");
        //  Enlever ombre
        m_meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        //  Enlever Collider
        m_boxCollider.enabled = false;
    }
}
