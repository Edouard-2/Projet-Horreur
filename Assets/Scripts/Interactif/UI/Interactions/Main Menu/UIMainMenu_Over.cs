using TMPro;
using UnityEngine;

public class UIMainMenu_Over : MonoBehaviour
{
    private Vector3 m_scale;
    
    [SerializeField, Tooltip("Le textMeshPro de l'objet")]
    private TextMeshPro m_textMeshPro;
    
    private void Awake()
    {
        m_scale = gameObject.transform.localScale;

        if (m_textMeshPro == null)
        {
            m_textMeshPro = GetComponent<TextMeshPro>();
            if (m_textMeshPro == null)
            {
                Debug.Log("Il faut mettre le textMeshPro !!");
            }
        }
    }

    private void OnMouseOver()
    {
        gameObject.transform.localScale = m_scale * 2;
    }

    private void OnMouseExit()
     {
         gameObject.transform.localScale = Vector3.one;
     }
 }