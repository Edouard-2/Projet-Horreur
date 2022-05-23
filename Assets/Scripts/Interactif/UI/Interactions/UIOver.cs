using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIOver : MonoBehaviour, IPointerExitHandler,IPointerEnterHandler
{
    private TextMeshProUGUI m_textMeshPro;
    private Color m_initColor;
    [SerializeField] private Color m_overColor;
    
    private void OnDisable()
    {
        if (m_textMeshPro == null) return;
        m_textMeshPro.color = m_initColor;
    }

    private void Awake()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        
        if (text == null) return;
        
        m_textMeshPro = text;
        m_initColor = text.color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_textMeshPro == null) return;
        m_textMeshPro.color = m_initColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_textMeshPro == null) return;
        m_textMeshPro.color = m_overColor;
    }
}