public class GameManager : Singleton<GameManager>
{
    public enum States
    {
        NULL,
        LOADING,
        MAIN_MENU,
        PLAYING,
        PAUSE,
        CINEMATIC
    }

    private States m_state;
    private States m_prevState;

    public States State => m_state;
    public States PrevState => m_prevState;

    public delegate void UiActivePauseGame(int p_isActive = 1);

    public UiActivePauseGame DoUiActivePauseGame;
    

    public void SwitchPauseGame()
    {
        m_prevState = m_state;
        
        //Si on était dans les option on revien dans le menu pause
        if (m_state == States.PAUSE && UIManager.Instance.m_isOption)
        {
            DoUiActivePauseGame?.Invoke(2);
            return;
        }
        if (m_state == States.PLAYING)
        {
            m_state = States.PAUSE;
            DoUiActivePauseGame?.Invoke();
            return;
        }

        m_state = States.PLAYING;
        DoUiActivePauseGame?.Invoke(0);
    }

    public void SetState(States p_state)
    {
        m_prevState = m_state;
        m_state = p_state;
    }

    protected override string GetSingletonName()
    {
        return "GameManager";
    }
}