using UnityEngine;

public class StartOrEndCollision : MonoBehaviour
{
    [SerializeField, Tooltip("Layer avec lequel le player est")] 
    private LayerMask m_layerPlayer;
    
    [Header("Start IA")]
    [SerializeField, Tooltip("Scriptable de l'event démarage et fin de l'IA")] 
    private EventsTrigger m_event;
    
    [Header("Position IA")]
    [SerializeField, Tooltip("Scriptable de l'event position")] 
    private EventsTriggerPos m_eventPos;
    
    [SerializeField, Tooltip("Position dans la scene ou l'IA va spawn")] 
    private Transform m_posIA;
    private void OnTriggerEnter(Collider other)
    {
        if ((m_layerPlayer.value & (1 << other.gameObject.layer)) > 0)
        {
            if (m_event == null)
            {
                m_eventPos.Raise(m_posIA.position);
                gameObject.SetActive(false);
                return;
            }
            m_event.Raise();
            gameObject.SetActive(false);
        }
    }
}
