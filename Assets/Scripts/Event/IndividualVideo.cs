using FMODUnity;
using UnityEngine;

public class IndividualVideo : MonoBehaviour
{
    [SerializeField, Tooltip("Event pour lancer / arreter la viedo")]
    private EventsTrigger m_event;
    
    [SerializeField, Tooltip("Le component emitter fmod")]
    private StudioEventEmitter m_emitter;


    private void OnEnable()
    {
        m_event.OnTrigger += DestroySelf;
    }

    private void DestroySelf(bool p_active)
    {
        if(p_active)m_emitter.Play();
        
        gameObject.SetActive(p_active);  
    }
}