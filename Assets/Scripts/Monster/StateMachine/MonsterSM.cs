using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSM : StateMachine
{
    //--------------WAYPOINTS--------------//
    [Header("WAYPOINTS")]
    [SerializeField, Tooltip("Tableau des waypoints du monstre")] 
    private List<Transform> m_waypointsArray;
    
    [SerializeField, Tooltip("Prefab pour creer un waypoint")] 
    private GameObject m_waypointPrefab;
    
    [SerializeField, Tooltip("Parent de tout les waypoint")] 
    private GameObject m_parentWaypoint;
    
    [SerializeField, Tooltip("Lorsque l'IA se déclenche elle va sur ce Way Point")] 
    private Transform m_wayPointStart;
    
    [SerializeField, Tooltip("Lorsque l'IA s'arrete elle va sur ce Way Point")] 
    private Transform m_wayPointEnd;
    
    //--------------IA--------------//
    [Header("IA")]
    [SerializeField, Tooltip("NavMeshAgent de l'IA monstre")] 
    private NavMeshAgent m_navMeshAgent;
    
    //--------------EVENTS--------------//
    [Header("EVENTS")]
    [SerializeField, Tooltip("Scriptable de l'event pour déclancher l'IA monstre")] 
    private EventsTrigger m_eventStart;
    
    [SerializeField, Tooltip("Scriptable de l'event pour arreter l'IA monstre")] 
    private EventsTrigger m_eventEnd;
    
    //--------------Other--------------//
    [Header("OTHER")]
    [SerializeField, Tooltip("LayerMask du joueur")] 
    public LayerMask m_layerPlayer;
    
    [SerializeField, Tooltip("Radius du champs de vision générale du monstre")] 
    public float m_radiusVision;
    
    [SerializeField, Tooltip("Angle Vertical de detection du joueur si le monstre est regardé par le joueur")] 
    [Range(1,180)] private float m_angleVertical = 10;
    
    [SerializeField, Tooltip("Angle Horizontal de detection du joueur si le monstre est regardé par le joueur")] 
    [Range(1,180)] private float m_angleHorizontal = 10;

    
    private bool m_startIA;
    
    private Transform m_currentWayPoint;
    private Transform m_prevWayPoint;
    
    //--------------STATE MACHINE--------------//
    [HideInInspector]
    public Pause m_pause;
    [HideInInspector]
    public Patrol m_patrol;
    [HideInInspector]
    public Hook m_hook;
    [HideInInspector]
    public Chase m_chase;
    [HideInInspector]
    public Escape m_escape;

    private void OnEnable()
    {
        m_eventStart.OnTrigger += StartIA;
        m_eventEnd.OnTrigger += EndIA;
    }

    private void OnDisable()
    {
        m_eventStart.OnTrigger -= StartIA;
        m_eventEnd.OnTrigger -= EndIA;
    }

    private void Awake()
    {
        if (m_navMeshAgent == null)
        {
            m_navMeshAgent = GetComponent<NavMeshAgent>();
            if (m_navMeshAgent == null)
            {
                Debug.LogError("Il faut mettre le Nav Mesh Agent sur L'IA !!!", this);
            }
        }
        
        m_pause = new Pause(this);
        m_patrol = new Patrol(this, m_waypointsArray,m_navMeshAgent,m_radiusVision, m_layerPlayer,m_angleHorizontal,m_angleVertical);
        m_hook = new Hook(this);
        m_chase = new Chase(this);
        m_escape = new Escape(this);
    }
    
    private void StartIA(bool p_idStart = true)
    {
        if (!p_idStart)
        {
            if (m_lastState == m_patrol)
            {
                NextState(m_patrol);
            }

            return;
        }
        NextState(m_patrol);
    }
    
    private void EndIA(bool p_idStart = true)
    {
        NextState(m_pause);
    }
    
    public void CreateWayPoint()
    {
        GameObject go = Instantiate(m_waypointPrefab, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(m_parentWaypoint.transform);
        m_waypointsArray.Add(go.transform);
    }

    protected override BaseState GetInitialState()
    {
        return m_pause;
    }
}
