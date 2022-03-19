using UnityEngine;

public class LootBox : MonoBehaviour, ILootBox
{
    [SerializeField, Tooltip("La clé du coffre")]
    private KeyType m_key;

    [SerializeField, Tooltip("L'animator du coffre")]
    private Animator m_doorAnimator;
    
    [SerializeField, Tooltip("Le nom du trigger de l'animation")]
    private string m_openName;

    private int m_openHash;

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

        if (m_key)
        {
            GetComponent<Renderer>().material.color = m_key.m_keyColor;
        }
    }

    public bool OpenChest(out KeyType o_key)
    {
        bool keyFounded = false;
        //Loot de la clé m_key 
        if (m_key == null)
        {
            Debug.Log("Pas de clé déso bb");
        }
        else
        {
            keyFounded = true;
            Debug.Log($"Tu a recu la clé {m_key}");
        }
        o_key = m_key;
        m_doorAnimator.SetTrigger(m_openHash);
        
        return keyFounded;
    }
    
}