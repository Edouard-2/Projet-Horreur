using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Mouvement
    [SerializeField, Tooltip("L'animator du player")]
    public Animator m_animator;

    [SerializeField, Tooltip("Le characontroller du player")]
    public CharacterController m_charaController;

    [SerializeField, Tooltip("La speed de déplacement du player")]
    public float m_speedMove = 10f;

    private Vector3 m_dir;
    private Vector3 m_velocity;

    [HideInInspector] public int m_moveHash = Animator.StringToHash("Move");
    [HideInInspector] public int m_idleHash = Animator.StringToHash("Idle");
    private bool m_runAnim = true;

    [HideInInspector] public float m_baseSpeed;

    private void Awake()
    {
        m_charaController = GetComponent<CharacterController>();
        m_baseSpeed = m_speedMove;
    }

    public void Mouvement()
    {
        //Mouvement sur le sol
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");

        //Simulation de gravité
        m_velocity.y += PlayerManager.Instance.Gravity * Time.deltaTime;
        m_charaController.Move(m_velocity * Time.deltaTime);

        if (xDir < 0.9f || xDir < -0.9f)
        {
            if (!m_runAnim)
            {
                m_runAnim = true;
                m_animator.ResetTrigger(m_moveHash);
                m_animator.SetTrigger(m_idleHash);
            }
        }
        if (yDir > 0.9f || yDir < -0.9f)
        {
            if (m_runAnim)
            {
                m_runAnim = false;
                m_animator.ResetTrigger(m_idleHash);
                m_animator.SetTrigger(m_moveHash);
            }
        }
        if (xDir == 0 && yDir == 0) return;

        float xMovement = xDir * m_speedMove * Time.deltaTime;
        float yMovement = yDir * m_speedMove * Time.deltaTime;

        if (!PlayerManager.Instance.m_isHooked)
        {
            m_dir = transform.right * xMovement + transform.forward * yMovement;
        }
        else
        {
            m_dir = transform.right * xMovement;
        }

        m_charaController.Move(m_dir);
    }
}