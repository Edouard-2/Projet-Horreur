using FMODUnity;
using UnityEngine;

public class UIMainMenu_PlayClick : MonoBehaviour
{
    [SerializeField, Tooltip("L'emitter qui va joueur le son du click")]
    private StudioEventEmitter m_emitterClick;
    
    [SerializeField, Tooltip("L'emitter qui va joueur le son quand on passe au dessus du texte")]
    private StudioEventEmitter m_emitterOver;

    private bool p_activeClick;
    private bool p_activeOver;
    private void OnMouseUpAsButton()
    {
        if (p_activeClick) return;
        p_activeClick = true;
        m_emitterClick.Play();
    }

    private void OnMouseOver()
    {
        if (p_activeOver) return;
        p_activeOver = true;
        m_emitterOver.Play();
    }

    private void OnMouseExit()
    {
        p_activeOver = false;
    }
}
