public class BaseState
{
    public string m_name;
    protected StateMachine m_stateMachine;

    public BaseState(string p_name, StateMachine p_stateMachine)
    {
        m_name = p_name;
        m_stateMachine = p_stateMachine;
    }

    public virtual void Enter(){}
    public virtual void UpdateLogic(){}
    public virtual void UpdateFunction(){}
    public virtual void UpdatePhysics(){}
    public virtual void Exit(){}
}