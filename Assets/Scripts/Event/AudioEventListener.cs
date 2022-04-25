using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class AudioEventListener : MonoBehaviour
{
    [SerializeField,Tooltip("L'event qui déclenchera le son")]private EventsTrigger m_event;
    [SerializeField,Tooltip("Le composent sound emitter")]private StudioEventEmitter m_soundEmitter;

    private void OnEnable()
    {
        m_event.OnTrigger += StartSound;
    }

    private void OnDisable()
    {
        m_event.OnTrigger += StartSound;
    }

    private void Awake()
    {
        if (m_soundEmitter == null)
        {
            m_soundEmitter = GetComponent<StudioEventEmitter>();
            if (m_soundEmitter == null)
            {
                Debug.LogError("Il faut mettre le SoundEmitter !!!", this);
            }
        }
    }

    private void StartSound(bool p_start = true)
    {
        m_soundEmitter.Play();
    }
}
