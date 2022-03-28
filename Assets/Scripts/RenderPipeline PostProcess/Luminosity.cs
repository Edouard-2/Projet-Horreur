using System;
using UnityEngine;

public class Luminosity : MonoBehaviour
{
    [SerializeField, Tooltip("Scriptableobjet avec la valeur de la Luminosité")] private UIOptionValue m_materialValue;
    [SerializeField, Tooltip("Matrial post process de la Luminosité")]private Material m_material;
    private float m_value;

    private void OnEnable()
    {
        m_materialValue.OnUpdateText += UpdateMaterial;
    }

    private void OnDisable()
    {
        m_materialValue.OnUpdateText -= UpdateMaterial;
        
        m_materialValue.InitValue();
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        m_value = (float)m_materialValue.GetIntValue() / 50;
        
        if (m_value == 0)
        {
            m_value = 0.01f;
        }
        
        m_material.SetFloat("_LuminosityStrength", m_value);
    }
}
