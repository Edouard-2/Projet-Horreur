using TMPro;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Image))]
public class UIActivation : MonoBehaviour
{
    [SerializeField, Tooltip("Image component")]
    private Image m_imageComponent;

    private Transform[] m_childsList;
    
    private void OnEnable()
    {
        UIManager.Instance.DoDisplayUIGamePause += SetVisible;
    }
    private void OnDisable()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.DoDisplayUIGamePause -= SetVisible;
        }
    }

    private void Awake()
    {
        if (m_imageComponent == null)
        {
            Debug.Log("pas image");
            m_imageComponent = GetComponent<Image>();
            if (m_imageComponent == null)
            {
                Debug.LogError("Faut mettre une image sur l'UI !!!");
            }
        }
        
        if (transform.childCount != 0)
        {
            m_childsList = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform trans = transform.GetChild(0);
                m_childsList[i] = trans;
            }
        }
    }

    private void SetVisible(bool p_active = true)
    {
        m_imageComponent.enabled = p_active;
        
        if (m_childsList != null)
        {
            for (int i = 0; i < m_childsList.Length; i++)
            {
                m_childsList[i].gameObject.SetActive(p_active);
            }
        }
    }
}