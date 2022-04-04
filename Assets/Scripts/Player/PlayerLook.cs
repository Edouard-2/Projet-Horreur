using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    //Mouvement de Vision
    [SerializeField, Tooltip("La speed de déplacement du player")]private float m_speedCursor = 10f;
    [SerializeField, Tooltip("La camera du joueur")]private Camera m_camera;
    private float m_mouseRotationX;
    private float m_mouseRotationY;
    private float m_xRotate = 0;

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