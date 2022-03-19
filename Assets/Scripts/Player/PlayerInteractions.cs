using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField, Tooltip("Le layer des portes")] private LayerMask m_layerDoor;
    [SerializeField, Tooltip("Le layer des coffres")] private LayerMask m_layerChest;

    [SerializeField, Tooltip("Trousseau de clé")]
    private List<KeyType> m_trousseauKey = new List<KeyType>();

    private void OnTriggerEnter(Collider other)
    {
        /*if ((m_layerChest.value & (1 << other.gameObject.layer)) > 0 )
        {
            LootBox myLootBox = other.GetComponent<LootBox>();
            if( myLootBox && myLootBox.OpenChest(out KeyType key))
            {
                if (!m_trousseauKey.Contains(key))
                {
                    m_trousseauKey.Add(key);
                }
            }
        }

        else if ((m_layerDoor.value & (1 << other.gameObject.layer)) > 0 ) 
        {
            Door myDoor =  other.GetComponent<Door>();
            if (myDoor)
            {
                myDoor.OpenDoor(m_trousseauKey, other.gameObject);
            }
        }*/
        
        
        
        
    }
}