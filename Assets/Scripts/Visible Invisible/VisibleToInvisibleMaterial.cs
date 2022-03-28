using UnityEngine;

public class VisibleToInvisibleMaterial : MonoBehaviour
{
    [SerializeField, Tooltip("False : Mettre le MeshRenderer de l'objet")]
    bool m_needMaterial;

    [SerializeField, Tooltip("Le material de l'objet")]
    Material m_material;

    private Renderer m_renderer;

    private void OnEnable()
    {
        PlayerManager.Instance.m_visionScript.DoChangeMaterial += SwitchInvisibleMaterial;
    }

    private void Awake()
    {
        if (m_material == null && m_needMaterial)
        {
            m_renderer = GetComponent<Renderer>();
            if (m_renderer != null)
            {
                m_material = m_renderer.material;
            }

            if (m_material == null)
            {
                Debug.LogWarning("Faut mettre le matérial sur l'objet stp !!!", this);
                PlayerManager.Instance.m_visionScript.DoChangeMaterial -= SwitchInvisibleMaterial;
                return;
            }
            SwitchInvisibleMaterial( -0.03f);
        }
    }

    private void SwitchInvisibleMaterial(float p_time)
    {
        if (m_needMaterial)
        {
            m_material.SetFloat("_StepStrenght", p_time);
        }
    }
}