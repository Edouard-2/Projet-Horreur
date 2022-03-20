using UnityEngine;

public class VisibleToInvisibleMaterial : MonoBehaviour
{
    [SerializeField, Tooltip("Le material de l'objet")] Material m_material;

    private void OnEnable()
    {
        PlayerManager.Instance.m_visionScript.DoChangeMaterial += SwitchInvisibleMaterial;
    }

    private void Awake()
    {
        if (m_material == null)
        {
            m_material = GetComponent<Renderer>().material;
            if (m_material == null)
            {
                Debug.LogError("Faut mettre le matérial sur l'objet stp !!!");
            }
        }
        m_material.SetFloat("_StepStrenght", -0.03f);
    }

    private void SwitchInvisibleMaterial(float p_time)
    {
        m_material.SetFloat("_StepStrenght", p_time);
    }
}
