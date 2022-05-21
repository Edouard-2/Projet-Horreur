using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState m_currentState;
    
    public BaseState m_lastState;

    private void Start()
    {
        m_currentState = GetInitialState();
        if (m_currentState != null)
        {
            m_currentState.Enter();
        }
    }

    private void Update()
    {
        if (m_currentState != null)
        {
            m_currentState.UpdateLogic();
            m_currentState.UpdateFunction();
            VerifyDeathPlayer();
        }
    }

    private void FixedUpdate()
    {
        if (m_currentState != null)
        {
            m_currentState.UpdatePhysics();
        }
    }

    public void NextState(BaseState p_newState)
    {
        m_currentState.Exit();
        
        m_lastState = m_currentState;
        
        m_currentState = p_newState;
        
        m_currentState.Enter();

    }
    
    protected virtual void VerifyDeathPlayer(){}

    protected virtual BaseState GetInitialState()
    {
        return null;
    }
}
