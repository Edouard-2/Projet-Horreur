using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField, Tooltip("Activer l'utilisation de la sauvegarde")]
    private bool m_checkpointActive;
    private void OnTriggerEnter(Collider other)
    {
        if (m_checkpointActive)
        {
            SaveSystem.ActiveSaveGame(m_checkpointActive);
        }
        
        SaveSystem.SavePlayer(PlayerManager.Instance);
        
        PlayerManager.Instance.SetCheckPoint(PlayerManager.Instance.transform.position);
        
        gameObject.SetActive(false);
    }
}
