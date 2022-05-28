using System;
using FMODUnity;
using UnityEngine;

public class IndividualVideo : MonoBehaviour
{
    [SerializeField, Tooltip("Event pour lancer / arreter la viedo")]
    private EventsTrigger m_event;
    
    [SerializeField, Tooltip("Le component emitter fmod")]
    private StudioEventEmitter m_emitter;
    
    [SerializeField, Tooltip("True : il n'est pas neutral")]
    private bool m_isInvisibleVisible;

    private VisibleInvisibleTimer m_InvisibleVisible;

    private int m_start;
    
    private void OnEnable()
    {
        m_event.OnTrigger += DestroySelf;
        
    }

    private void Awake()
    {
        if (m_isInvisibleVisible)
        {
            m_InvisibleVisible = GetComponent<VisibleInvisibleTimer>();
        }
    }

    private void SemiDestroy(bool p_active)
    {
        if (m_isInvisibleVisible)
        {
            m_InvisibleVisible.m_render.enabled = p_active;
            m_InvisibleVisible.enabled = p_active;
            m_InvisibleVisible.m_render.enabled = p_active;
        }
    }

    private void DestroySelf(bool p_active)
    {
        if (m_start > 2) return;
        
        if (m_start == 1 && m_isInvisibleVisible)
        {
            Debug.Log("Step 1", this);
            SemiDestroy(false);
            m_start++;
            return;
        }
        
        if (p_active)
        {
            if (m_start == 0)
            {
                m_emitter.Play();
            }
            
        }
        
        if (m_start == 2)
        {
            Debug.Log("Step 2", this);
            SemiDestroy(true);
        }
        
        m_start++;
        
        gameObject.SetActive(p_active);  
    }
}