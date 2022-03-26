using UnityEngine;

[CreateAssetMenu(fileName = "UIOptionValue", menuName = "UIOptionValue", order = 1)]
public class UIOptionValue : ScriptableObject
{
    [Range(0,100)]public int m_volumeInit;
    private int m_volume;

    public delegate void UpdateText(); 
    public UpdateText OnUpdateText;

    public string GetStringValue()
    {
        return m_volume.ToString();
    }
    public void SetValue(int p_value)
    {
        m_volume += p_value;
        
        if (m_volume < 0)
        {
            m_volume = 0;
        }
        else if (m_volume > 100)
        {
            m_volume = 100;
        }
    }

    public void InitValue()
    {
        m_volume = m_volumeInit;
    }
}
