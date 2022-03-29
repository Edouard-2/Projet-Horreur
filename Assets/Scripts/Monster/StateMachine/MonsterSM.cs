using UnityEngine;

public class MonsterSM : StateMachine
{
    [HideInInspector]
    public Patrol m_patrol;
    [HideInInspector]
    public Hook m_Hook;
    [HideInInspector]
    public Chase m_chase;
    [HideInInspector]
    public Escape m_escape;

    private void Awake()
    {
        m_patrol = new Patrol(this);
        m_Hook = new Hook(this);
        m_chase = new Chase(this);
        m_escape = new Escape(this);
    }

    protected override BaseState GetInitialState()
    {
        return m_patrol;
    }
}
