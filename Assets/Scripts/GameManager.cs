using UnityEngine;

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

    public States State => m_state;

    public delegate void UiActivePauseGame(bool p_isActive = true);

    public UiActivePauseGame DoUiActivePauseGame;

    public void SwitchPauseGame()
    {
        if (m_state == States.PLAYING)
        {
            m_state = States.PAUSE;
            DoUiActivePauseGame?.Invoke();
            return;
        }
        m_state = States.PLAYING;
        DoUiActivePauseGame?.Invoke(false);
    }
    
    public void SetState( States p_state)
    {
        m_state = p_state;
    }
    
    protected override string GetSingletonName()
    {
        return "GameManager";
    }
}
