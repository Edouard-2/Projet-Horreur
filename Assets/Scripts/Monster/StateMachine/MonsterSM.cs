using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSM : StateMachine
{
    //--------------WAYPOINTS--------------//
    [Header("WAYPOINTS")] 
    [SerializeField, Tooltip("Tableau des waypoints du monstre")]
    public List<Transform> m_waypointsLvl1;
    [SerializeField, Tooltip("Tableau des waypoints du monstre")]
    public List<Transform> m_waypointsLvl2;
    [SerializeField, Tooltip("Tableau des waypoints du monstre")]
    public List<Transform> m_waypointsLvl3;
    [SerializeField, Tooltip("Tableau des waypoints du monstre")]
    public List<Transform> m_waypointsLvl4;

    public List<List<int>> p_aeinr;
    [SerializeField, Tooltip("Prefab pour creer un waypoint")]
    private GameObject m_waypointPrefab;

    [SerializeField, Tooltip("Parent de tout les waypoint")]
    private GameObject m_parentWaypoint;

    [SerializeField, Tooltip("Lorsque l'IA se déclenche elle va sur ce Way Point")]
    private Transform m_wayPointStart;

    [SerializeField, Tooltip("Lorsque l'IA s'arrete elle va sur ce Way Point")]
    private Transform m_wayPointEnd;
    public List<List<Transform>> m_waypointsArray;
    private int m_indexLevelWaypoint;

    //--------------IA--------------//
    [Header("IA")] 
    [SerializeField, Tooltip("NavMeshAgent de l'IA monstre")]
    public NavMeshAgent m_navMeshAgent;
    
    [SerializeField, Tooltip("collider capsule de l'IA monstre")]
    public CapsuleCollider m_collider;
    
    [SerializeField, Tooltip("Transform de la tête du monstre")]
    public Transform m_headTransform;

    //--------------EVENTS--------------//
    [Header("EVENTS")] 
    [SerializeField, Tooltip("True: Le monstre commence par Idle")]
    private bool m_isIdleStart;

    [SerializeField, Tooltip("Scriptable de l'event Alerter avec le son l'IA monstre")]
    private EventsTriggerPos m_eventAlertSound;
    
    [SerializeField, Tooltip("Scriptable de l'event pour placer l'IA monstre")]
    private List<EventsTriggerPos> m_eventPosList;

    [SerializeField, Tooltip("Scriptable de l'event pour déclancher l'IA monstre")]
    private List<EventsTrigger> m_eventStart;

    [SerializeField, Tooltip("Scriptable de l'event pour arreter l'IA monstre")]
    private List<EventsTrigger> m_eventEnd;

    //--------------Other--------------//
    [Header("ANIMATIONS")] 
    [SerializeField, Tooltip("Component animator")]
    public Animator m_animator;
    
    private const string m_moving = "Moving";
    private const string m_retract = "Retract";
    private const string m_mesmer = "Mesmer";
    private const string m_bruit = "Bruit";

    [HideInInspector]public int m_movingHash;
    [HideInInspector]public int m_retractHash;
    [HideInInspector]public int m_mesmerHash;
    [HideInInspector]public int m_bruitHash;
    
    private int m_currentHash;
    
    //--------------Other--------------//
    [Header("OTHER")] 
    [SerializeField, Tooltip("LayerMask du joueur")]
    public LayerMask m_layerPlayer;

    [SerializeField, Tooltip("Speed de déplacement du joueur et du monstre lorsque le monstre hook le joueur")]
    public float m_speedHook;

    [SerializeField, Tooltip("Radius du champs de vision générale du monstre")]
    public float m_radiusVision;

    [SerializeField, Tooltip("Radius de detection du joueur si il est dans cette zone")]
    public float m_radiusDetection;

    [SerializeField, Tooltip("Angle Vertical de detection du joueur si le monstre est regardé par le joueur")]
    [Range(1, 180)]
    private float m_angleVertical = 10;

    [SerializeField, Tooltip("Angle Horizontal de detection du joueur si le monstre est regardé par le joueur")]
    [Range(1, 180)]
    private float m_angleHorizontal = 10;

    [HideInInspector] public Vector3 m_soundAlertPosition;
    
    private bool m_startIA;

    [HideInInspector]public bool m_isPlayerDead;
    
    private bool m_isStartIA;

    private Vector3 m_initPos;

    private Transform m_currentWayPoint;
    private Transform m_prevWayPoint;

    //--------------STATE MACHINE--------------//
    public Pause m_pause;
    public Patrol m_patrol;
    public Hook m_hook;
    public Chase m_chase;
    public Idle m_idle;
    public Defense m_defense;
    public AlertSound m_alertSound;

    private void OnEnable()
    {
        m_eventAlertSound.OnTrigger += SoundAlertIA;

        PlayerManager.Instance.UpdateFirstPos += SetInitPos;
        
        foreach (EventsTriggerPos elem in m_eventPosList)
        {
            elem.OnTrigger += SetPosIA;
        }
        foreach (EventsTrigger elem in m_eventStart)
        {
            elem.OnTrigger += StartIA;
        }
        foreach (EventsTrigger elem in m_eventEnd)
        {
            elem.OnTrigger += EndIA;
        }
    }

    private void OnDisable()
    {
        m_eventAlertSound.OnTrigger -= SoundAlertIA;
        
        PlayerManager.Instance.UpdateFirstPos -= SetInitPos;
        
        foreach (EventsTriggerPos elem in m_eventPosList)
        {
            elem.OnTrigger -= SetPosIA;
        }
        foreach (EventsTrigger elem in m_eventStart)
        {
            elem.OnTrigger -= StartIA;
        }
        foreach (EventsTrigger elem in m_eventEnd)
        {
            elem.OnTrigger -= EndIA;
        }
    }

    private void Awake()
    {
        TurnOffAI();
        
        m_initPos = transform.position;
        
        m_isPlayerDead = false;
        
        //Récupérer le navMeshAgent si null
        if (m_navMeshAgent == null)
        {
            m_navMeshAgent = GetComponent<NavMeshAgent>();
            if (m_navMeshAgent == null)
            {
                Debug.LogError("Il faut mettre le Nav Mesh Agent sur L'IA !!!", this);
            }
        }
        
        //Récupérer le collider si null
        if (m_collider == null)
        {
            m_collider = GetComponent<CapsuleCollider>();
            if (m_collider == null)
            {
                Debug.LogError("Il faut mettre le collider capsule sur L'IA !!!", this);
            }
        }
        //Initialisation des Triggers animations
        m_mesmerHash = Animator.StringToHash(m_mesmer);
        m_movingHash = Animator.StringToHash(m_moving);
        m_bruitHash = Animator.StringToHash(m_bruit);
        m_retractHash = Animator.StringToHash(m_retract);
        
        //Initialisations des states
        m_pause = new Pause(this);
        m_idle = new Idle(this);
        m_patrol = new Patrol(this, m_waypointsArray[m_indexLevelWaypoint], m_layerPlayer, m_angleHorizontal, m_angleVertical);
        m_hook = new Hook(this, m_speedHook);
        m_chase = new Chase(this);
        m_defense = new Defense(this);
        m_alertSound = new AlertSound(this);
    }

    private void SetInitPos()
    {
        NextState(m_pause);
        m_navMeshAgent.nextPosition = m_initPos;
        NextState(m_patrol);
    }

    private void SoundAlertIA(Vector3 p_pos)
    {
        m_alertSound.m_FirstPos = p_pos;
        NextState(m_alertSound);
    }

    private void TurnOffAI()
    {
        m_navMeshAgent.SetDestination(transform.position);
        m_currentState = m_pause;
        m_collider.enabled = false;
        transform.GetChild(0)?.gameObject.SetActive(false);
    }

    private void SetPosIA(Vector3 p_pos)
    {
        Debug.Log("position");
        
        m_navMeshAgent.nextPosition = p_pos;
    }

    private void StartIA(bool p_idStart = true)
    {
        m_isStartIA = true;
        m_collider.enabled = true;
        transform.GetChild(0)?.gameObject.SetActive(true);
        
        if (m_lastState != null && m_lastState != m_pause)
        {
            NextState(m_lastState);
            return;
        }

        if (m_isIdleStart)
        {
            NextState(m_idle);
            return;
        }
        NextState(m_patrol);
    }

    private void EndIA(bool p_idStart = true)
    {
        m_isStartIA = false;
        NextState(m_pause);
        m_indexLevelWaypoint++;
        if (m_indexLevelWaypoint > 3) return;
        m_patrol.m_wayPointsList = m_waypointsArray[m_indexLevelWaypoint];
    }

    public void SetNewAnimation(int p_hash)
    {
        if (p_hash == m_currentHash) return;
        
        m_animator.ResetTrigger(m_currentHash);
        m_animator.SetTrigger(p_hash);
        m_currentHash = p_hash;
    }

    public void CreateWayPoint()
    {
        GameObject go = Instantiate(m_waypointPrefab, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(m_parentWaypoint.transform);
        m_waypointsArray[m_indexLevelWaypoint].Add(go.transform);
    }

    protected override void VerifyDeathPlayer()
    {
        if (Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) < m_radiusDetection
            && !m_isPlayerDead
            && m_isStartIA)
        {
            m_isPlayerDead = true;
            m_hook.AddIndexSpeed(0);
            NextState(m_pause);
            PlayerManager.Instance.Death();
        }
    }

    protected override BaseState GetInitialState()
    {
        return m_pause;
    }
}