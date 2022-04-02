using UnityEngine;

public class StartOrEndCollision : MonoBehaviour
{
    [SerializeField, Tooltip("Layer avec lequel le player est")] 
    private LayerMask m_layerPlayer;

    [SerializeField, Tooltip("Scriptable de l'event")] 
    private EventsTrigger m_event;
    private void OnTriggerEnter(Collider other)
    {
        if ((m_layerPlayer.value & (1 << other.gameObject.layer)) > 0)
        {
            m_event.Raise();
            gameObject.SetActive(false);
        }
    }
}
