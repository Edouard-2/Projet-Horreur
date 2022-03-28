using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MonsterManager : Singleton<MonsterManager>
{
    [SerializeField, Tooltip("Tableau des waypoints du monstre")] private List<Transform> m_waypointsArray;
    [SerializeField, Tooltip("Préfab pour créer un waypoint")] private GameObject m_waypointPrefab;
    [SerializeField, Tooltip("Parent de tout les waypoint")] private GameObject m_parentWaypoint;
    
    [SerializeField, Tooltip("Lorsque l'IA se déclenche elle va sur ce Way Point")] private Transform m_wayPointStart;
    [SerializeField, Tooltip("Lorsque l'IA s'arrête elle va sur ce Way Point")] private Transform m_wayPointEnd;

    [SerializeField, Tooltip("NavMeshAgent de l'IA monstre")] private NavMeshAgent m_navMeshAgent;
    
    private bool m_startIA;
    
    private Transform m_currentWayPoint;
    private Transform m_prevWayPoint;

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
    }
    
    public void StartIA()
    {
        Debug.Log("Activation monstre");
        m_startIA = true;
        transform.position = new Vector3(m_wayPointStart.position.x,transform.position.y,m_wayPointStart.position.z);
    }

    public void EndIA()
    {
        Debug.Log("Désactivation monstre");
        m_startIA = false;
        transform.position = new Vector3(m_wayPointEnd.position.x,transform.position.y,m_wayPointEnd.position.z);
    }

    private void Update()
    {
        if (m_startIA)
        {
            //Les trucs de l'IA
            if (m_waypointsArray.Count != 0)
            {
                if (m_currentWayPoint == null || Vector3.Distance(transform.position, m_currentWayPoint.position) < 1)
                {
                    if (m_currentWayPoint != m_prevWayPoint)
                    {
                        m_prevWayPoint = m_currentWayPoint;
                    }
                    
                    m_currentWayPoint = GetRandomWayPoint();
                }
                m_navMeshAgent.SetDestination(m_currentWayPoint.position);
            }
        }
    }

    private Transform GetRandomWayPoint()
    {
        Transform wayPoint;
        
        List<Transform> arrayWayPoint = new List<Transform>();

        for (int i = 0; i < m_waypointsArray.Count; i++)
        {
            arrayWayPoint.Add(m_waypointsArray[i]);
        }
        
        if (m_currentWayPoint != null)
        {
            arrayWayPoint.Remove(m_currentWayPoint);
            if( m_prevWayPoint != null)
            {
                arrayWayPoint.Remove(m_prevWayPoint);
            }
            Debug.Log(arrayWayPoint);
            Debug.Log(m_waypointsArray);
        }
        
        wayPoint = m_waypointsArray[Random.Range(0, m_waypointsArray.Count)];

        return wayPoint;
    }

    public void CreateWayPoint()
    {
        GameObject go = Instantiate(m_waypointPrefab, Vector3.zero, quaternion.identity);
        go.transform.SetParent(m_parentWaypoint.transform);
        m_waypointsArray.Add(go.transform);
    }

    protected override string GetSingletonName()
    {
        return "MonsterManager";
    }
}