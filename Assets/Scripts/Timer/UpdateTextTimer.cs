using TMPro;
using UnityEngine;

public class UpdateTextTimer : MonoBehaviour
{
    [SerializeField, Tooltip("TextMeshPro component")]
    private TextMeshPro m_textMeshPro;

    private bool start;
    
    private void OnEnable()
    {
        //TimerManager.Instance.UpdateTextHandler += UpdateText;
    }
    private void OnDisable()
    {
        //TimerManager.Instance.UpdateTextHandler -= UpdateText;
    }

    private void Awake()
    {
        if (m_textMeshPro == null)
        {
            m_textMeshPro = GetComponent<TextMeshPro>();
            if (m_textMeshPro == null)
            {
                Debug.LogError("Faut mettre le textmeshpro !!", this);
            }
        }
    }

    private void UpdateText(string p_text)
    {
        m_textMeshPro.text = p_text;
    }
}
