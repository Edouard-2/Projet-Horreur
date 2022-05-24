using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIActivation))]
public class UIQuitGame : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Tooltip("Emitter du son Click")] private StudioEventEmitter m_clickEmitter;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        m_clickEmitter.Play();
        //Debug.Log("Quit Game");
        Application.Quit();
    }
}