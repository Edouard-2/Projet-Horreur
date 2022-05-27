using TMPro;
using UnityEngine;

public class VisibleInvisibleTimer : MonoBehaviour
{
    [SerializeField, Tooltip("Script sur le timer ? checked: true")] bool m_isTimer;
    [SerializeField, Tooltip("Checked: Visible to Invisible")] bool m_isVisibleInvisible;
    
    private TextMeshPro m_test;
    private MeshRenderer m_render;

    private void Awake()
    {
        if(m_isTimer) m_test = GetComponent<TextMeshPro>();
        else m_render = GetComponent<MeshRenderer>();

        SwitchMaterials(m_isVisibleInvisible);
    }

    private void OnEnable()
    {
        //PlayerManager.Instance.DoVisibleToInvisibleHandler += SwitchMaterials;
    }

    private void SwitchMaterials(bool p_visible = false)
    {
        Debug.Log(p_visible);
        if (m_isVisibleInvisible)
        {
            if (m_isTimer) m_test.enabled = p_visible;
            else m_render.enabled = p_visible;
            return;
        }
        if (m_isTimer) m_test.enabled = !p_visible;
        else m_render.enabled = !p_visible;
    }
}