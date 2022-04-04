using System;
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
    [SerializeField, Tooltip("La camera du joueur")]private Camera m_camera;
    private float m_mouseRotationX;
    private float m_mouseRotationY;
    private float m_xRotate;

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

        if (!PlayerManager.Instance.m_isHooked)
        {
            m_xRotate -= m_mouseRotationY;
            m_xRotate = Mathf.Clamp(m_xRotate, -90f, 90f);

            m_camera.transform.localRotation = Quaternion.Euler(m_xRotate,0,0);
            transform.Rotate(Vector3.up * m_mouseRotationX);
        }
        else
        {
            
        }

    }
}