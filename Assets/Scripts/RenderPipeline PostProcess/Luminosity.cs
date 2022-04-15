using UnityEngine;

public class Luminosity : MonoBehaviour
{
    [Header("Event")]
    [SerializeField, Tooltip("Scriptableobjet avec la valeur de la Luminosité")] private UIOptionValue m_materialValue;
    
    [Header("Material")]
    [SerializeField, Tooltip("Matrial post process de la Luminosité")]private Material m_material;
    private float m_value;

    [Header("Vignette")]
    [SerializeField, Tooltip("La strength de la vignette")] [Range(0, 10)]
    private float m_vignetteStrength; 
    
    [Header("Lut Table")]
    [SerializeField, Tooltip("Est ce que la lut est changé par le gradient")]
    private bool m_isGradientLut; 
    [SerializeField, Tooltip("Gradient qui changera la lut table")]
    private Gradient m_gradientLut; 
    [SerializeField, Tooltip("Taille de la lut table")]
    private Vector2Int m_lutTextSize; 
    [SerializeField, Tooltip("La texture de la lut table")]
    private Texture2D m_lutTexture;
    [SerializeField, Tooltip("Transition de la lut table")][Range(0,1)]
    private float m_lutTransition;

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

    private void OnValidate()
    {
        //Debug.Log("Validate");
        if (m_isGradientLut) GenerateLutTexture();
        
        m_material.SetFloat("_VignetteStrength", m_vignetteStrength);
        m_material.SetFloat("_LutColorTransition", m_lutTransition);
    }

    private void GenerateLutTexture()
    {
        m_lutTexture = new Texture2D(m_lutTextSize.x, m_lutTextSize.y);
        m_lutTexture.wrapMode = TextureWrapMode.Clamp;

        for (int i = 0; i < m_lutTextSize.x; i++)
        {
            Color color = m_gradientLut.Evaluate(i / (float) m_lutTextSize.x);
            for (int j = 0; j < m_lutTextSize.y; j++)
            {   
                m_lutTexture.SetPixel(i,j,color);
            }
        }
        
        m_lutTexture.Apply();
        m_material.SetTexture("_LutColorGradeTex", m_lutTexture);
    }
    
    private void UpdateMaterial()
    {
        m_value = (float)m_materialValue.GetIntValue() / 25;
        
        if (m_value == 0)
        {
            m_value = 0.01f;
        }
        
        m_material.SetFloat("_LuminosityStrength", m_value);
    }
}
