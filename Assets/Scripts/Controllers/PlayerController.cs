using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("Le characontroller du player")]private CharacterController m_charaController;
    [SerializeField, Tooltip("La speed de déplacement du player")]private float m_speedMove = 10f;
    [SerializeField, Tooltip("La speed de déplacement du player")]private float m_speedCursor = 10f;
    [SerializeField, Tooltip("La camera du joueur")]private Camera m_camera;

    private Vector3 m_dir;
    
    private float m_mouseRotationX;
    private float m_mouseRotationY;
    private float m_xRotate = 0f;

    private void Awake()
    {
        m_charaController = GetComponent<CharacterController>();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            DoMouvement();
        
        DoCursorMouvement();
    }

    private void DoCursorMouvement()
    {
        m_mouseRotationX = Input.GetAxis("Mouse X") * m_speedCursor;
        m_mouseRotationY =  Input.GetAxis("Mouse Y") * m_speedCursor;

        m_xRotate -= m_mouseRotationY;
        m_xRotate = Mathf.Clamp(m_xRotate, -90f, 90f);
        
        transform.Rotate(Vector3.up * m_mouseRotationX);
        
        m_camera.transform.localRotation = Quaternion.Euler(m_xRotate,0,0);
        
    }

    private void DoMouvement()
    {
        float xDir = Input.GetAxis("Horizontal") * m_speedMove * Time.deltaTime;
        float yDir = Input.GetAxis("Vertical") * m_speedMove * Time.deltaTime;
        
        m_dir = m_camera.transform.right * xDir + m_camera.transform.forward * -yDir;
        
        m_charaController.Move(new Vector3(m_dir.x, 0, m_dir.y));
    }
}