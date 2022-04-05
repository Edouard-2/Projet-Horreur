using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
public class UIDisplayHideEvent : MonoBehaviour
{
    [Header("Events")]
    [SerializeField, Tooltip("L'event qui déclenchera la fonction")]
    private EventsTrigger m_event;
    
    [Header("Image")]
    [SerializeField, Tooltip("L'event qui déclenchera la fonction")]
    private Image m_image;

    [Header("Animation")]
    [SerializeField, Tooltip("Est ce que l'obj à une animation")]
    private bool m_isAnimation;
    
    [SerializeField, Tooltip("Animator de l'obj")]
    private Animator m_animator;
    
    [SerializeField, Tooltip("Temps avant que l'obj soit destroy")]
    private float m_waitEndValue = 1;
    
    private WaitForSeconds m_waitEndAnimation;

    private void OnEnable()
    {
        m_event.OnTrigger += DisplayOrHideUI;
    }
    private void OnDisable()
    {
        m_event.OnTrigger -= DisplayOrHideUI;
    }

    private void Awake()
    {
        //Initialisation du wait for second
        m_waitEndAnimation = new WaitForSeconds(m_waitEndValue);
        
        if (m_image == null)
        {
            m_image = GetComponent<Image>();
        }
        if (m_animator == null)
        {
            m_animator = GetComponent<Animator>();
        }
    }

    private void DisplayOrHideUI(bool p_bool)
    {
        //Si il y a une animation
        if (m_isAnimation)
        {
            if (p_bool)
            {
            
                m_image.enabled = true;
                return;
            }
            m_image.enabled = true;
            StartCoroutine(HideObject());
            return;
        }
        
        //Sinon désactivation simple
        if (p_bool)
        {
            m_image.enabled = true;
            return;
        }
        m_image.enabled = true;
        gameObject.SetActive(false);
        
    }

    IEnumerator HideObject()
    {
        yield return m_waitEndAnimation;
        gameObject.SetActive(false);
    }
}
