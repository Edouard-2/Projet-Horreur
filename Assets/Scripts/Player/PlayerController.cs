using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Mouvement
    [SerializeField, Tooltip("Le characontroller du player")]private CharacterController m_charaController;
    [SerializeField, Tooltip("La speed de déplacement du player")]private float m_speedMove = 10f;
    private Vector3 m_dir;
    private Vector3 m_velocity;
  
    private void Awake()
    {
        m_charaController = GetComponent<CharacterController>();
    }

    public void Mouvement()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_speedMove *= 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            m_speedMove /= 2;
        }
        
        //Mouvement sur le sol
        float xDir = Input.GetAxis("Horizontal") * m_speedMove * Time.deltaTime;
        float yDir = Input.GetAxis("Vertical") * m_speedMove * Time.deltaTime;
        
        m_dir = transform.right * xDir + transform.forward * yDir;
        
        m_charaController.Move(m_dir);

        //Simulation de gravité
        m_velocity.y += PlayerManager.Instance.Gravity * Time.deltaTime;
        m_charaController.Move(m_velocity * Time.deltaTime);
    }
}