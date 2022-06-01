using System;
using System.Collections;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField, Tooltip("Scriptableobject qui contient les informations sur le son global")]
    private UIOptionValue m_soundGlobal;
    
    [SerializeField, Tooltip("Scriptableobject qui contient les informations sur la Musique")]
    private UIOptionValue m_soundMusique;
    
    [SerializeField, Tooltip("Scriptableobject qui contient les informations sur les SFX")]
    private UIOptionValue m_soundVFX;
    
    [SerializeField, Tooltip("Emitter de l'ambiance labo in game")]
    public StudioEventEmitter m_ambiance1;
    [SerializeField, Tooltip("Emitter de l'ambiance labo in game")]
    public StudioEventEmitter m_ambiance2;
    
    private float m_globalVolume;
    private float m_musiqueVolume;
    private float m_VFXVolume;
    
    private Bus m_master;
    public Bus m_musique;
    private Bus m_vfx;
    
    private void OnEnable()
    {
        m_soundGlobal.OnUpdateText += UpdateSoundVolumeGlobal;
        m_soundMusique.OnUpdateText += UpdateSoundVolumeMusique;
        m_soundVFX.OnUpdateText += UpdateSoundVolumeVFX;
    }

    private void OnDisable()
    {
        m_soundGlobal.OnUpdateText -= UpdateSoundVolumeGlobal;
        m_soundMusique.OnUpdateText -= UpdateSoundVolumeMusique;
        m_soundVFX.OnUpdateText -= UpdateSoundVolumeVFX;
    }

    private void Awake()
    {
        m_master = RuntimeManager.GetBus("bus:/Master");
        m_musique = RuntimeManager.GetBus("bus:/Master/Musique");
        m_vfx = RuntimeManager.GetBus("bus:/Master/SFX");
    }

    private void Start()
    {
        m_soundGlobal.SetValue(m_soundGlobal.m_valueInit);
        m_soundMusique.SetValue(m_soundMusique.m_valueInit);
        m_soundVFX.SetValue(m_soundVFX.m_valueInit);
    }

    private void UpdateSoundVolumeGlobal()
    {
        m_globalVolume = (float)m_soundGlobal.GetIntValue() / 100;
        
        Debug.Log(m_globalVolume);
        m_master.setVolume(m_globalVolume);
    }

    public void UpdateSoundVolumeMusique()
    {
        m_musiqueVolume = (float)m_soundMusique.GetIntValue() / 100;
        
        Debug.Log(m_musiqueVolume);
        m_musique.setVolume(m_musiqueVolume);
    }

    private void UpdateSoundVolumeVFX()
    {
        m_VFXVolume = (float)m_soundVFX.GetIntValue() / 100;
        
        Debug.Log(m_VFXVolume);
        m_vfx.setVolume(m_VFXVolume);
    }
    
    /// <summary>
    /// Faire monter ou baisser le son progressivement
    /// </summary>
    /// <param name="m_event"> Event emitter sur lequel on baisse le son </param>
    /// <param name="p_in"> True : FadeIn / False : FadeOut </param>
    public static void FadeOut(StudioEventEmitter m_event, bool p_in)
    {
        Debug.Log($"Je vais baisser le son de : {m_event}");
        int dir = 1;
        if (p_in) dir = 0;
        m_event.SetParameter("Fade", dir);
    }
    
    protected override string GetSingletonName()
    {
        return "SoundManager";
    }
}