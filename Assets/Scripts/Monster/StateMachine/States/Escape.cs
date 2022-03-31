using System.Collections;
using UnityEngine;
public class Escape : BaseState
{
    private MonsterSM m_sm;
    private Vector3 m_remoteWaypoint;
    WaitForSeconds m_waitSecond = new WaitForSeconds(3f);

    public Escape(MonsterSM p_stateMachine) : base("Escape", p_stateMachine)
    {
        m_sm = p_stateMachine;
    }
    
     public override void Enter()
    {
        Debug.Log("ESCAPE");
    }

    public override void UpdateLogic()
    {
        if (PlayerManager.Instance.m_visionScript.m_isBlurVision == 1)
        {
            m_sm.NextState(m_sm.m_patrol);
        }
    }

    public override void UpdateFunction()
    {
        //Chercher le waypoint le plus éloigner
        m_remoteWaypoint = LocateRemoteWaypoint(PlayerManager.Instance.transform.position);
        m_sm.m_navMeshAgent.SetDestination(m_remoteWaypoint);
        
        //Si la vision entre le monstre et le joueur est caché je disparait
        if (Vector3.Distance(m_sm.transform.position, m_remoteWaypoint) < 1)
        {
            float m_distPlayer = Vector3.Distance(m_remoteWaypoint, PlayerManager.Instance.transform.position);
            Ray ray = new Ray(m_remoteWaypoint, (m_remoteWaypoint - PlayerManager.Instance.transform.position).normalized * m_distPlayer);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, m_distPlayer))
            {
                HideMonster();
            }
        }
    }

    private void HideMonster()
    {
        m_sm.StartCoroutine(DisplayeMonster());
        m_sm.gameObject.SetActive(false);
    }
    
    IEnumerator DisplayeMonster()
    {
        yield return m_waitSecond;
        m_sm.gameObject.SetActive(true);
        //Se TP
        m_sm.transform.position = LocateRemoteWaypoint(PlayerManager.Instance.transform.position);
        //Retour en patrol
        m_sm.NextState(m_sm.m_patrol);
    }

    private Vector3 LocateRemoteWaypoint(Vector3 p_targetRemote, bool p_isRaycast = false )
    {
        Vector3 chooseWayPoint = p_targetRemote;
        Vector3 remotePos = new Vector3();

        for (int i = 0; i < m_sm.m_waypointsArray.Count; i++)
        {
            float distPlayer = Vector3.Distance(chooseWayPoint,p_targetRemote);
            float newDistPlayer = Vector3.Distance(m_sm.m_waypointsArray[i].transform.position,p_targetRemote);
            
            if (newDistPlayer > distPlayer)
            {
                remotePos = m_sm.m_waypointsArray[i].transform.position;
                
                if (p_isRaycast)
                {
                    Ray ray = new Ray(m_sm.m_waypointsArray[i].transform.position,
                        (m_sm.m_waypointsArray[i].transform.position - p_targetRemote).normalized * newDistPlayer);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, newDistPlayer))
                    {
                        if ((m_sm.m_layerPlayer.value & (1 << hit.collider.gameObject.layer)) <= 0)
                        {
                            chooseWayPoint = m_sm.m_waypointsArray[i].transform.position;
                        }
                        else if(chooseWayPoint == p_targetRemote)
                        {
                            chooseWayPoint = remotePos;
                        }
                    }
                }
                else
                {
                    chooseWayPoint = remotePos;
                }
            }
        }
        return chooseWayPoint;
    }

    public override void Exit()
    {
        m_sm.m_lastState = this;
    }
}