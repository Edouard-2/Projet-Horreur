using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField, Tooltip("ScriptableObj pour activer l'utilisation de la sauvegarde")]
    private IsFirstCheckpointActive m_checkpointActive;
    private void OnTriggerEnter(Collider other)
    {
        if(m_checkpointActive != null)
        {
            m_checkpointActive.m_isActive = true;
        }
        
        SaveSystem.SavePlayer(PlayerManager.Instance);
        
        PlayerManager.Instance.SetCheckPoint(PlayerManager.Instance.transform.position);
        
        gameObject.SetActive(false);
    }
}
