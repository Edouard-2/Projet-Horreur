using System;
using UnityEngine;

public class KeyOutline : MonoBehaviour
{
    [SerializeField, Tooltip("Le mesh renderer")]
    private MeshRenderer m_renderer;
    
    [SerializeField, Tooltip("Le mesh renderer")]
    LayerMask m_layer;
    
    private void Awake()
    {
        m_renderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        if ((m_layer.value & (1 << other.gameObject.layer)) > 0)
        {
            m_renderer.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        if ((m_layer.value & (1 << other.gameObject.layer)) > 0)
        {
            m_renderer.enabled = false;
        }
    }
}
