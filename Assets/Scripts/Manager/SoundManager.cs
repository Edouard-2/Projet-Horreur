using System;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField, Tooltip("Scriptableobject qui contient les informations sur le son global")]
    private UIOptionValue m_soundGlobal;
    
    [SerializeField, Tooltip("Scriptableobject qui contient les informations sur le son VFX")]
    private UIOptionValue m_soundVFX;

    private float m_globalVolume;
    private float m_VFXVolume;

    private void OnEnable()
    {
        m_soundGlobal.OnUpdateText += UpdateSoundVolumeGlobal;
        m_soundVFX.OnUpdateText += UpdateSoundVolumeVFX;
    }

    private void OnDisable()
    {
        m_soundGlobal.OnUpdateText -= UpdateSoundVolumeGlobal;
        m_soundVFX.OnUpdateText -= UpdateSoundVolumeVFX;
    }

    private void UpdateSoundVolumeGlobal()
    {
        m_globalVolume = (float)m_soundGlobal.GetIntValue() / 100;
        Debug.Log(m_globalVolume);
    }

    private void UpdateSoundVolumeVFX()
    {
        m_VFXVolume = (float)m_soundVFX.GetIntValue() / 100;
        Debug.Log(m_VFXVolume);
    }

    protected override string GetSingletonName()
    {
        return "SoundManager";
    }
}
