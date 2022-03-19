using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IDoor
{
    [SerializeField, Tooltip("Type de clés")]
    private KeyType m_neededKey;

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
            GetComponent<Renderer>().material.color = m_neededKey.m_keyColor;
        }
    }

    public void OpenDoor(List<KeyType> p_playerKeys, GameObject p_door)
    {
        if (m_neededKey)
        {
            //Si le joueur n'a pas la clé necessaire
            if (p_playerKeys == null || !p_playerKeys.Contains(m_neededKey))
            {
                Debug.Log($"La cle {m_neededKey.name} est nécessaire");
                return;
            }
        }
        //On ouvre la porte
        Debug.Log("Je m'ouvre");
        m_doorAnimator?.SetTrigger(m_openHash);
    }
}