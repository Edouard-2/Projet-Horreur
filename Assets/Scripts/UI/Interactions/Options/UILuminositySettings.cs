using TMPro;
using UnityEngine;

public class UILuminositySettings : MonoBehaviour
{
    [SerializeField, Tooltip("Le scryptable objet qui contient la valeur, et qui sera lier au boutton UI")]
    private UIOptionValue m_optionValue;
    
    [SerializeField, Tooltip("Le textMeshPro sur l'obj")]
    private TextMeshProUGUI m_textMeshPro;

    private void Awake()
    {
        if (m_textMeshPro == null)
        {
            m_textMeshPro = GetComponent<TextMeshProUGUI>();
            if (m_textMeshPro == null)
            {
                Debug.LogError("Ya pas le Textmeshpro sur l'UI", this);
            }
        }

        //Initialiser le scriptableObj
        m_optionValue.InitValue();
        
        //Initialisé le Text
        UpdateText();
    }

    private void OnEnable()
    {
        m_optionValue.OnUpdateText += UpdateText;
    }

    private void OnDisable()
    {
        m_optionValue.OnUpdateText -= UpdateText;
    }

    private void UpdateText()
    {
        m_textMeshPro.SetText(m_optionValue.GetStringValue());
        
        //Changer la luminosité
    }
}
