using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Mouvement de Vision
    [SerializeField, Tooltip("La speed de déplacement du player")]private float m_speedCursor = 10f;
    [SerializeField, Tooltip("La camera du joueur")]private Camera m_camera;

    //Constante
    private const float m_gravity = -9.81f;
    
    //Mouvement
    [SerializeField, Tooltip("Le characontroller du player")]private CharacterController m_charaController;
    [SerializeField, Tooltip("La speed de déplacement du player")]private float m_speedMove = 10f;
    private Vector3 m_dir;
    private Vector3 m_velocity;
    private float m_mouseRotationX;
    private float m_mouseRotationY;
    private float m_xRotate = 0f;
    
    //Changement de vision
    [SerializeField, Tooltip("Courbe de pourcentage de flou au changement de vision")] private AnimationCurve m_curveVision;
    [SerializeField, Tooltip("Material de flou pour le postprocess")] private Material m_matVision;
    private float m_timeVision;
    private bool m_resetTimeVision = false;

    private void Awake()
    {
        m_charaController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //Mouvement du Joueur
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            DoMouvement();
        
        //Rotation de la camera en fonction du curseur
        DoCursorMouvement();
        
        //Changement de vision
        if (Input.GetKeyDown(KeyCode.U))
        {
            m_timeVision = Time.time;
            m_resetTimeVision = true;
        }
        DoSwitchView();
    }

    private void DoSwitchView()
    {
        
        if (Time.time - m_timeVision >= 5)
        {
            m_resetTimeVision = false;
            return;
        }
        if (m_resetTimeVision)
        {
            Debug.Log(m_curveVision.Evaluate(  Time.time - m_timeVision));
            Debug.Log(Time.time - m_timeVision);
            m_matVision.SetFloat("_BlurSize",Time.time - m_timeVision);
        }
    }

    private void DoCursorMouvement()
    {
        //Mouvement de la vue
        m_mouseRotationX = Input.GetAxis("Mouse X") * m_speedCursor;
        m_mouseRotationY =  Input.GetAxis("Mouse Y") * m_speedCursor;

        m_xRotate -= m_mouseRotationY;
        m_xRotate = Mathf.Clamp(m_xRotate, -90f, 90f);
        
        m_camera.transform.localRotation = Quaternion.Euler(m_xRotate,0,0);
        transform.Rotate(Vector3.up * m_mouseRotationX);
    }

    private void DoMouvement()
    {
        //Mouvement sur le sol
        float xDir = Input.GetAxis("Horizontal") * m_speedMove * Time.deltaTime;
        float yDir = Input.GetAxis("Vertical") * m_speedMove * Time.deltaTime;
        
        m_dir = transform.right * xDir + transform.forward * yDir;
        
        m_charaController.Move(m_dir);

        //Simulation de gravité
        m_velocity.y += m_gravity * Time.deltaTime;
        m_charaController.Move(m_velocity * Time.deltaTime);
    }
}