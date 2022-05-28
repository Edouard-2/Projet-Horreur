using System.Collections;
using FMODUnity;
using UnityEngine;

public class TimerManager : Singleton<TimerManager>
{
    [SerializeField, Tooltip("Emitter du son de fin de timer")]
    private StudioEventEmitter m_flashEmitter;
    
    [SerializeField, Tooltip("Emitter du son d'alarm de fin")]
    private StudioEventEmitter m_alarmEmitter;
    
    [SerializeField, Tooltip("Animator du fade blanc")]
    private Animator m_animatorFade;
    
    [SerializeField, Tooltip("Event du timer")]
    private EventsTrigger m_event;
    
    [SerializeField, Tooltip("Event de la dernière video")]
    private EventsTrigger m_eventLastVideo;
    
    [SerializeField, Tooltip("Le timer est de X minutes")]
    private int m_minuteStart;

    private WaitForSeconds m_waitOneSeconde = new WaitForSeconds(.5f);
    
    private bool m_isRunning;
    private bool m_isStart;
    
    private readonly int m_fadeHash = Animator.StringToHash("FadeIn");
    private readonly int m_idleHash = Animator.StringToHash("FadeOut");
    
    private int m_timerMinuteValue;
    private int m_timerHourValue;
    private bool m_isAlarmRun;
    private string m_valueString;

    private Coroutine m_currentCoroutine;

    public delegate void UpdateText(string p_text);

    public event UpdateText UpdateTextHandler;
    
    public string StringValue => m_valueString;
    
    private void OnEnable()
    {
        if (m_event == null) return;
        m_event.OnTrigger += StartOrStopTimer;
    }
    
    private void OnDisable()
    {
        if (m_event == null) return;
        m_event.OnTrigger -= StartOrStopTimer;
    }

    public void StartOrStopTimer(bool p_isStart)
    {
        if (p_isStart && !m_isStart)
        {
            ResetTimer();
            PauseOrRestartTimer(p_isStart);
            return;
        }
        m_isStart = false;
        
        PauseOrRestartTimer(p_isStart);
    }

    public void ResetTimer()
    {
        m_isStart = true;
        m_timerHourValue = m_minuteStart;
        m_timerMinuteValue = 0;
    }
    
    /// <summary>
    /// relancer ou mettre en pause le timer
    /// </summary>
    /// <param name="p_isStart"> true: lancer / flase: arrêter</param>
    public void PauseOrRestartTimer(bool p_isStart)
    {
        if (p_isStart)
        {
            m_isRunning = true;
            m_alarmEmitter.Stop();
            m_currentCoroutine = StartCoroutine(IncreaseTime());
            return;
        }
        
        m_isRunning = false;
        
        if (m_currentCoroutine != null)
        {
            StopCoroutine(m_currentCoroutine);
        }
    }
    
    //Minuteur
    private void UpdateTimerValue()
    {
        if(!m_isRunning || !m_isStart) return;

        m_timerMinuteValue--;

        if (m_timerHourValue == 0 && m_timerMinuteValue <= 20)
        {
            //Lancer le son de l'alarm
            if (!m_isAlarmRun)
            {
                m_isAlarmRun = true;
                m_alarmEmitter.Play();
            }
        }
        if (m_timerHourValue == 0 && m_timerMinuteValue == 50)
        {
            //Lancer la 4eme video
            m_eventLastVideo?.Raise();
        }
        
        if (m_timerMinuteValue <= 0)
        {

            if (m_timerHourValue == 40)
            {
                //Lancer la 4eme video
                m_eventLastVideo?.Raise();
            }
            
            if (m_timerHourValue == 0)
            {
                StartCoroutine(Death());
                return;
            }
            m_timerHourValue--;
            m_timerMinuteValue = 59;
        }
        
        UpdateStringValue();
        
        m_currentCoroutine = StartCoroutine(IncreaseTime());
    }

    //Mettre les Minutes et secondes en strings pour les textes qui l'utiliserons
    private void UpdateStringValue()
    {
        string hours = "";
        string minute = "";
        
        if (m_timerHourValue < 10) hours = "0";
        if (m_timerMinuteValue < 10) minute = "0";

        hours += m_timerHourValue.ToString();
        minute += m_timerMinuteValue.ToString();
        
        m_valueString = $"{hours}:{minute}";
        
        UpdateTextHandler?.Invoke(m_valueString);
    }

    IEnumerator Death()
    {
        m_isAlarmRun = false;
        
        //Arrêter le son
        SoundManager.Instance.m_musique.setVolume(0);
        yield return new WaitForSeconds(2f);
        
        m_flashEmitter.Play();
        m_alarmEmitter.Stop();
        
        //fade in blanc
        m_animatorFade.ResetTrigger(m_idleHash);
        m_animatorFade.SetTrigger(m_fadeHash);
        
        yield return new WaitForSeconds(1f);
        //Mort
        PlayerManager.Instance.Death();
        
        yield return new WaitForSeconds(2f);
        //Reset le fade in
        m_animatorFade.ResetTrigger(m_fadeHash);
        m_animatorFade.SetTrigger(m_idleHash);
    }

    IEnumerator IncreaseTime()
    {
        yield return m_waitOneSeconde;
        UpdateTimerValue();
    }

    protected override string GetSingletonName()
    {
        return "TimerManager";
    }
}
