using TMPro;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    //Mouvement de Vision

    //Other
    [Header("Event")]
    [SerializeField, Tooltip("Sensibilité du joueur")]
    public UIOptionValue m_eventSensivity;
    
    [Header("Other")]
    [SerializeField, Tooltip("La speed de déplacement du player")]private float m_speedCursor = 5f;
    [SerializeField, Tooltip("La camera du joueur")]public Camera m_camera;
    [SerializeField, Tooltip("L'animator de la rotation de camera du joueur")]public Animator m_animator;
    private float m_mouseRotationX;
    private float m_mouseRotationY;
    private float m_xRotate;

    [HideInInspector]public int m_idleHash = Animator.StringToHash("Idle");
    [HideInInspector]public int m_leftHash = Animator.StringToHash("Left");
    [HideInInspector]public int m_rightHash = Animator.StringToHash("Right");

    private void OnEnable()
    {
        m_eventSensivity.OnUpdateText += UpdateSensitivity;
    }

    private void OnDisable()
    {
        m_eventSensivity.OnUpdateText -= UpdateSensitivity;
    }

    private void UpdateSensitivity()
    {
        m_speedCursor = (float)m_eventSensivity.GetIntValue() / 4;
    }
    
    public void CursorMouvement()
    {
        //Mouvement de la vue
        m_mouseRotationX = Input.GetAxis("Mouse X") * m_speedCursor;
        m_mouseRotationY =  Input.GetAxis("Mouse Y") * m_speedCursor;

        if (PlayerManager.Instance.m_isHooked)return;

        m_animator.ResetTrigger(m_idleHash);
        m_animator.ResetTrigger(m_leftHash);
        m_animator.SetTrigger(m_leftHash);
        
        if (m_mouseRotationX < 0)
        {
            m_animator.ResetTrigger(m_leftHash);
            m_animator.ResetTrigger(m_idleHash);
            m_animator.SetTrigger(m_rightHash);
        }
        
        if (m_mouseRotationX == 0)
        {
            m_animator.ResetTrigger(m_rightHash);
            m_animator.ResetTrigger(m_leftHash);
            m_animator.SetTrigger(m_idleHash);
        }
        
        m_xRotate -= m_mouseRotationY;
        m_xRotate = Mathf.Clamp(m_xRotate, -90f, 90f);

        m_camera.transform.localRotation = Quaternion.Euler(m_xRotate,0,0);
        transform.Rotate(Vector3.up * m_mouseRotationX);
    }
}