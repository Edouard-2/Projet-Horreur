using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IDoor
{
    [SerializeField, Tooltip("Type de clés")]
    public KeyType m_neededKey;

    [SerializeField, Tooltip("L'animator de la porte")] 
    private Animator m_doorAnimator;

    [SerializeField]private string m_openName = "open";
    private int m_openHash;

    private void Awake()
    {
        if (m_doorAnimator == null)
        {
            m_doorAnimator = GetComponent<Animator>();
            if(m_doorAnimator == null)
            {
                Debug.Log("Gros chien l'animator", this);
            }
        }
        m_openHash = Animator.StringToHash(m_openName);
        if (m_neededKey)
        {
            GetComponent<Renderer>().material = m_neededKey.m_doorMat;
            m_neededKey.m_doorMat.SetFloat("_isAim", 0);
            m_neededKey.m_keyMat.SetFloat("_isAim", 0);
        }
    }

    public bool OpenDoor(KeyType p_playerKey, GameObject p_door)
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
        //On ouvre la porte
        Debug.Log("Je m'ouvre");
        m_doorAnimator?.SetTrigger(m_openHash);
        return true;
    }
}