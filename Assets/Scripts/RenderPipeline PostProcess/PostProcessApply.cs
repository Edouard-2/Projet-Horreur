using System;
using UnityEngine;

public class PostProcessApply : MonoBehaviour
{
    [Header("Event")]
    [SerializeField, Tooltip("Scriptableobjet avec la valeur de la Luminosité")] private UIOptionValue m_materialValue;
    
    [Header("Material")]
    [SerializeField, Tooltip("Matrial post process")]private Material m_material;
    private float m_value;

    [Header("Vignette")]
    [SerializeField, Tooltip("La strength de la vignette")] [Range(0.65f, 1)]
    public float m_vignetteStrength; 
    [SerializeField, Tooltip("Lorsque la vignette apparait sur l'écran c'est sa vitesse de fade in")] [Range(0, 0.5f)]
    public float m_vignetteStepMultiplier = 0.25f;
    [SerializeField, Tooltip("Strength max que la vignette pourra avoir dans le jeu")] [Range(0, 1)]
    public float m_vignetteStepMax = 0.88f;

    [HideInInspector]public float m_vignetteInitValue;
    [HideInInspector]public float m_vignetteStartValue;
    
    [Header("Depth")]
    [SerializeField, Tooltip("La strength de la vignette")] [Range(0, 1)]
    public float m_depthStrenght; 
    
    [Header("Lut Table")]
    [SerializeField, Tooltip("Est ce que la lut est changé par le gradient")]
    private bool m_isGradientLut; 
    [SerializeField, Tooltip("Gradient qui changera la lut table")]
    private Gradient m_gradientLut; 
    [SerializeField, Tooltip("Taille de la lut table")]
    private Vector2Int m_lutTextSize; 
    [SerializeField, Tooltip("La texture de la lut table")]
    private Texture2D m_lutTexture;
    [Tooltip("Transition de la lut table")] [Range(0,1)]
    public float m_lutTransition;

    private void OnEnable()
    {
        m_materialValue.OnUpdateText += UpdateMaterial;
    }

    private void OnDisable()
    {
        m_materialValue.OnUpdateText -= UpdateMaterial;
    }

    private void Awake()
    {
        m_vignetteInitValue = m_vignetteStrength;
        m_materialValue.InitValue();
        UpdateMaterial();
    }

    private void OnValidate()
    {
        //Debug.Log("Validate");
        if (m_isGradientLut) GenerateLutTexture();

        UpdateVignette();
        UpdateLutTable();
        UpdateDepth();

    }

    public void UpdateDepth()
    {
        m_material.SetFloat("_DepthLevel", m_depthStrenght);
    }
    
    public void UpdateLutTable()
    {
        m_material.SetFloat("_LutColorTransition", m_lutTransition);
    }

    public void UpdateVignette()
    {
        m_material.SetFloat("_VignetteStrength", m_vignetteStrength);
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
