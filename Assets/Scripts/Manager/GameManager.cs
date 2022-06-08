using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private States m_state;
    private States m_prevState;

    public States State => m_state;
    public States PrevState => m_prevState;

    public delegate void UiActivePauseGame(int p_isActive = 1);

    public UiActivePauseGame DoUiActivePauseGame;

    private void OnEnable()
    {
        m_reInstance = false;
        m_state = States.PLAYING;
    }

    public enum States
    {
        NULL,
        LOADING,
        MAIN_MENU,
        PLAYING,
        PAUSE,
        DEATH
    }

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
            //TimerManager.Instance.PauseOrRestartTimer(false);
            
            PlayerManager.Instance.m_visionScript.StopAllCoroutines();
            PlayerManager.Instance.m_visionScript.m_timeStopBlind = Time.time;

            MonsterSM.Instance.Stop();
            m_state = States.PAUSE;
            DoUiActivePauseGame?.Invoke();
            return;
        }
        
        //TimerManager.Instance.PauseOrRestartTimer(true);
        if (!PlayerManager.Instance.m_visionScript.m_blindActive)
        {
            PlayerManager.Instance.m_visionScript.StopOrStartBlindEffects();
        }
        else
        {
            StartCoroutine(PlayerManager.Instance.m_visionScript.ActiveBlindEffectDepth(-1, 0.0008f));
            
            PlayerManager.Instance.m_visionScript.m_postProcessScript.m_vignetteStrength = PlayerManager.Instance.m_visionScript.m_postProcessScript.m_vignetteStartValue;
        }
        
        MonsterSM.Instance.Relaunch();
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