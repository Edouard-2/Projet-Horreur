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
    private StudioEventEmitter m_ambiance;
    
    private float m_globalVolume;
    private float m_musiqueVolume;
    private float m_VFXVolume;
    
    private Bus m_master;
    private Bus m_musique;
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

    private void UpdateSoundVolumeGlobal()
    {
        m_globalVolume = (float)m_soundGlobal.GetIntValue() / 100;
        
        Debug.Log(m_globalVolume);
        m_master.setVolume(m_globalVolume);
    }

    private void UpdateSoundVolumeMusique()
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

    protected override string GetSingletonName()
    {
        return "SoundManager";
    }
}
