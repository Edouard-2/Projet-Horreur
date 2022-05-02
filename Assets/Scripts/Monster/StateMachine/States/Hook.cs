using UnityEngine;
public class Hook : BaseState
{
    private MonsterSM m_sm;
    private float m_speedHook;
    private float initTime;

    private Quaternion m_playerInitRot;

    public Hook(MonsterSM p_stateMachine, float p_speedHook) : base("Hook", p_stateMachine)
    {
        m_sm = p_stateMachine;
        m_speedHook = p_speedHook;
    }
    
     public override void Enter()
    {
        Debug.Log("HOOK");
        
        PlayerManager.Instance.m_isHooked = true;

        m_playerInitRot = PlayerManager.Instance.transform.rotation;
        
        m_sm.SetNewAnimation(m_sm.m_mesmerHash);
        
        m_sm.m_navMeshAgent.SetDestination(m_sm.transform.position);
    }

    public override void UpdateLogic()
    {
        if (PlayerManager.Instance.m_visionScript.m_isBlurVision == 0)
        {
            m_sm.NextState(m_sm.m_defense);
            return;
        }
        
        Vector3 vectorPlayerMonster = (PlayerManager.Instance.transform.position - m_sm.transform.position).normalized;
        Ray ray = new Ray(m_sm.transform.position,vectorPlayerMonster);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, m_sm.m_radiusVision))
        {
            if((m_sm.m_layerPlayer.value & (1 << hit.collider.gameObject.layer)) <= 0)
            {
                m_sm.NextState(m_sm.m_chase);
                Debug.DrawRay(m_sm.transform.position,vectorPlayerMonster * Vector3.Distance(hit.point,m_sm.transform.position),Color.red,3);
            }
            Debug.DrawRay(m_sm.transform.position,vectorPlayerMonster * Vector3.Distance(hit.point,m_sm.transform.position),Color.blue);
        }
    }

    public override void UpdateFunction()
    {
        if (initTime == 0)
        {
            initTime = Time.time;
        }
        float tTime = Time.time - initTime;
        
        //Attirer le joueur vers sois même et nous même avancer vers lui
        PlayerManager.Instance.transform.position = Vector3.MoveTowards(PlayerManager.Instance.transform.position, m_sm.m_headTransform.position,tTime/m_speedHook);
        
        //Le joueur et le monstre se fixent
        PlayerManager.Instance.transform.LookAt(m_sm.m_headTransform.position);
        
        m_sm.transform.LookAt(PlayerManager.Instance.transform);
    }

    public override void Exit()
    {
        m_sm.m_lastState = this;
        
        PlayerManager.Instance.m_isHooked = false ;
        
        PlayerManager.Instance.transform.rotation = m_playerInitRot;
        
        m_sm.m_navMeshAgent.SetDestination(m_sm.transform.position);
        
        initTime = 0;
    }
}