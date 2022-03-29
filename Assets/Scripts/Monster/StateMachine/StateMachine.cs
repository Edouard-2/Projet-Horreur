using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState m_currentState;

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

        m_currentState = p_newState;
        
        m_currentState.Enter();

    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }
}
