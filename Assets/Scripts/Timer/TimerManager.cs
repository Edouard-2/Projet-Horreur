using System;
using System.Collections;
using UnityEngine;

public class TimerManager : Singleton<TimerManager>
{
    [SerializeField, Tooltip("Event du timer")]
    private EventsTrigger m_event;
    
    [SerializeField, Tooltip("Le timer est de X minutes")]
    private int m_minuteStart;

    private WaitForSeconds m_waitOneSeconde = new WaitForSeconds(1);
    
    [HideInInspector]public bool m_isRunning;
    
    private int m_timerMinuteValue;
    private int m_timerHourValue;
    private string m_valueString;
    
    public string StringValue => m_valueString;
    
    private void OnEnable()
    {
        m_timerHourValue = m_minuteStart;
        
        if (m_event == null) return;
        m_event.OnTrigger += StartOrEndTimer;
    }
    private void OnDisable()
    {
        if (m_event == null) return;
        m_event.OnTrigger -= StartOrEndTimer;
    }

    /// <summary>
    /// Lancer ou arrêter le timer
    /// </summary>
    /// <param name="p_isStart"> true: lancer / flase: arrêter</param>
    public void StartOrEndTimer(bool p_isStart)
    {
        if (p_isStart)
        {
            m_isRunning = true;
            StartCoroutine(IncreaseTime());
            return;
        }
        
        m_isRunning = false;
        StopAllCoroutines();
    }
    
    private void UpdateTimerValue()
    {
        if(!m_isRunning) return;

        m_timerMinuteValue--;

        if (m_timerMinuteValue <= 0)
        {
            if (m_timerHourValue == 0)
            {
                PlayerManager.Instance.Death();
                return;
            }
            m_timerHourValue--;
            m_timerMinuteValue = 59;
        }

        UpdateStringValue();
        
        Debug.Log(m_valueString);
        
        StartCoroutine(IncreaseTime());
    }

    private void UpdateStringValue()
    {
        
        string hours = "";
        string minute = "";
        
        if (m_timerHourValue < 10) hours = "0";
        if (m_timerMinuteValue < 10) minute = "0";

        hours += m_timerHourValue.ToString();
        minute += m_timerMinuteValue.ToString();
        
        m_valueString = $"{hours} : {minute}";
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
