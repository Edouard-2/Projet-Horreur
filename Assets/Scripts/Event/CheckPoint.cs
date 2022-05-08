using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField, Tooltip("ScriptableObj pour activer l'utilisation de la sauvegarde")]
    private List<IsFirstCheckpointActive> m_checkpointActive;
    private void OnTriggerEnter(Collider other)
    {
        SaveSystem.SavePlayer(PlayerManager.Instance);
        
        PlayerManager.Instance.SetCheckPoint(PlayerManager.Instance.transform.position);
        
        if(m_checkpointActive.Count != 0)
        {
            foreach (IsFirstCheckpointActive elem in m_checkpointActive)
            {
                elem.m_isActive = true;
            }
        }
        
        gameObject.SetActive(false);
    }
}
