using UnityEngine;

[CreateAssetMenu(fileName = "UIOptionValue", menuName = "UIOptionValue", order = 1)]
public class UIOptionValue : ScriptableObject
{
    [Range(0,100)]public int m_valueInit;
    private int m_value;

    public delegate void UpdateText(); 
    public UpdateText OnUpdateText;

    public string GetStringValue()
    {
        return m_value.ToString();
    }
    
    public int GetIntValue()
    {
        return m_value;
    }
    public void SetValue(int p_value)
    {
        m_value += p_value;
        
        if (m_value < 0)
        {
            m_value = 0;
        }
        else if (m_value > 100)
        {
            m_value = 100;
        }
    }

    public void InitValue()
    {
        m_value = m_valueInit;
    }
}
