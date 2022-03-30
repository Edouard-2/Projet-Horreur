using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Mouvement
    [SerializeField, Tooltip("Le characontroller du player")]private CharacterController m_charaController;
    [SerializeField, Tooltip("La speed de déplacement du player")]public float m_speedMove = 10f;
    private Vector3 m_dir;
    private Vector3 m_velocity;
    [HideInInspector]public float m_baseSpeed;
  
    private void Awake()
    {
        m_charaController = GetComponent<CharacterController>();
        m_baseSpeed = m_speedMove;
    }

    public void Mouvement()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //Mouvement sur le sol
            float xDir = Input.GetAxis("Horizontal") * m_speedMove * Time.deltaTime;
            float yDir = Input.GetAxis("Vertical") * m_speedMove * Time.deltaTime;

            m_dir = transform.right * xDir + transform.forward * yDir;

            m_charaController.Move(m_dir);
        }
        //Simulation de gravité
        m_velocity.y += PlayerManager.Instance.Gravity * Time.deltaTime;
        m_charaController.Move(m_velocity * Time.deltaTime);
    }
}