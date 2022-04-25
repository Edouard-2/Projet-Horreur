using UnityEngine;
using UnityEngine.Rendering;
using FMOD.Studio;
using FMODUnity;

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
    private int m_layer;

    private StudioEventEmitter m_test;
    
    private void OnEnable()
    {
        PlayerManager.Instance.DoVisibleToInvisibleHandler += DoVisibleToInvisible;
        if (m_isVisibleToInvisible)
        {
            PlayerManager.Instance.DoSwitchLayer += DoSwitchLayerVisible;
        }
    }

    private void Awake()
    {
        m_layer = gameObject.layer;
        
        if(m_boxCollider == null && m_needCollider)
        {
            m_boxCollider = GetComponent<BoxCollider>();
            
            if(m_boxCollider == null)
            {
                Debug.LogError("Remplit le collider Gros Chien !!!", this);
            }
        }
        
        if(m_meshRenderer == null && m_needRenderer)
        {
            m_meshRenderer = GetComponent<MeshRenderer>();
            
            if(m_meshRenderer == null)
            {
                Debug.LogError("Remplit le Renderer Gros Chien !!!", this);
            }
        }
        
        DoVisibleToInvisible(true);
    }

    private void DoSwitchLayerVisible(bool p_start)
    {
        if (p_start)
        {
            if ((m_layer == LayerMask.NameToLayer("Invisibility")) )
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else if ((m_layer == LayerMask.NameToLayer("VisibleToInvisibleDoor")) )
            {
                gameObject.layer = LayerMask.NameToLayer("Doors");
            }
            else if ((m_layer == LayerMask.NameToLayer("VisibleToInvisibleKey")))
            {
                gameObject.layer = LayerMask.NameToLayer("Keys");
            }
            else if ((m_layer ==  LayerMask.NameToLayer("InvisibleToVisibleTrasvaseur")))
            {
                gameObject.layer = LayerMask.NameToLayer("Transvaseur");
            }

            return;
        }
        gameObject.layer = m_layer;
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
        if (m_needRenderer)
        {
            m_meshRenderer.shadowCastingMode = ShadowCastingMode.On;
        }
        //  Mettre Collider
        if (m_needCollider)
        {
            m_boxCollider.enabled = true;
        }
    }

    void Hide()
    {
        //  Enlever ombre
        if (m_needRenderer)
        {
            m_meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }

        //  Enlever Collider
        if (m_needCollider)
        {
            m_boxCollider.enabled = false;
        }
    }
}
