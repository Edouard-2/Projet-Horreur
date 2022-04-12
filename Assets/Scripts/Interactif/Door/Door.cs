using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField, Tooltip("Type de clés")]
    public KeyType m_neededKey;
    
    [SerializeField, Tooltip("Event du sound")]
    public EventsTriggerPos m_soundEvent;

    [SerializeField, Tooltip("L'animator de la porte")]
    private Animator m_doorAnimator;

    [SerializeField, Tooltip("Layermask du monstre")]
    private LayerMask m_layerMonstre;

    [SerializeField, Tooltip("Nom du trigger pour déclancher l'animation Open")]
    private string m_openName = "Open";

    [SerializeField, Tooltip("Nom du trigger pour déclancher l'animation Close")]
    private string m_closeName = "Close";

    private int m_openHash;
    private int m_closeHash;

    private bool m_isOpen;

    [SerializeField, Tooltip("Material de la Porte")]
    public Material m_doorMat;

    private void Awake()
    {
        if (m_doorAnimator == null)
        {
            m_doorAnimator = GetComponent<Animator>();
            if (m_doorAnimator == null)
            {
                Debug.Log("Gros chien l'animator", this);
            }
        }

        m_openHash = Animator.StringToHash(m_openName);
        m_closeHash = Animator.StringToHash(m_closeName);
        if (m_neededKey)
        {
            GetComponent<Renderer>().material = m_neededKey.m_doorMat;
            m_neededKey.m_doorMat.SetFloat("_isAim", 0);
            m_neededKey.m_keyMat.SetFloat("_isAim", 0);
            m_doorMat = m_neededKey.m_doorMat;
        }
        else
        {
            if (m_doorMat == null)
            {
                Debug.LogError("Faut mettre le material dans les portes scripts !!!", this);
            }
        }
        
        m_doorMat.SetFloat("_isAim", 0);
    }

    public bool OpenDoor(KeyType p_playerKey)
    {
        if (m_neededKey)
        {
            //Si le joueur n'a pas la clé necessaire
            if (p_playerKey == null || p_playerKey != m_neededKey)
            {
                Debug.Log($"La cle {m_neededKey.name} est nécessaire");
                return false;
            }
        }

        if (!m_isOpen)
        {
            //On ouvre la porte
            Debug.Log("Je m'ouvre");
            m_doorAnimator.SetTrigger(m_openHash);
            
            m_soundEvent.Raise(PlayerManager.Instance.transform.position);
            
            m_isOpen = true;
        }

        return true;
    }

    public void CloseDoor(Transform p_target)
    {
        if (m_isOpen)
        {
            Debug.Log("Close");
            m_doorAnimator.SetTrigger(m_closeHash);
            m_isOpen = false;
            return;
        }

        if ((m_layerMonstre.value & (1 << p_target.gameObject.layer)) > 0)
        {
            m_doorAnimator.SetTrigger(m_openHash);
        }
    }
}