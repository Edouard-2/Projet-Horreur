using UnityEngine.UI;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField, Tooltip("Le layer des portes")] private LayerMask m_layerDoor;
    [SerializeField, Tooltip("Le layer des coffres")] private LayerMask m_layerChest;

    [SerializeField, Tooltip("Trousseau de clé")] private KeyType m_trousseauKey;
    [SerializeField, Tooltip("UI de la clés ingame")] private Image m_KeyUI;

    private GameObject m_keyObject;

    public void VerifyLayer(Transform p_target)
    {
        if ((m_layerChest.value & (1 << p_target.gameObject.layer)) > 0 )
        {
            LootBox myLootBox = p_target.GetComponent<LootBox>();
            if( myLootBox && myLootBox.OpenChest(out KeyType key))
            {
                if (m_trousseauKey == null)
                {
                    m_trousseauKey = key;
                    m_keyObject = p_target.gameObject;
                    SetUIKey(myLootBox);
                }
                else
                {
                    //Enlever la précedente clé
                    EjectKey();
                    m_trousseauKey = key;
                    m_keyObject = p_target.gameObject;
                    SetUIKey(myLootBox);
                }
            }
        }

        else if ((m_layerDoor.value & (1 << p_target.gameObject.layer)) > 0 ) 
        {
            Door myDoor =  p_target.GetComponent<Door>();
            if (myDoor)
            {
                if (myDoor.OpenDoor(m_trousseauKey, p_target.gameObject))
                {
                    m_trousseauKey = null;
                    StartCoroutine(m_keyObject.GetComponent<LootBox>().DestroySelf());
                    m_KeyUI.color = Color.clear;
                }
            }
        }
        else
        {
            Debug.Log("Rien pour intéragir");
        }
    }

    private void SetUIKey(LootBox p_key)
    {
        Debug.Log(p_key.m_key.m_keyColor);

        m_KeyUI.color = new Vector4(p_key.m_key.m_keyColor.r,p_key.m_key.m_keyColor.g,p_key.m_key.m_keyColor.b,1);

    }
    
    private void EjectKey()
    {
        //Ejecter la clé
        Debug.Log("clear");
        m_keyObject.transform.position = transform.position + transform.forward;
        m_KeyUI.color = Color.clear;
    }
}