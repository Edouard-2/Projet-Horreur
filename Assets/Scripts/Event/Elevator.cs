using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Level")]
    [SerializeField, Tooltip("Autre lvl à charger")]
    private GameObject m_nextLevel;
    
    [SerializeField, Tooltip("level à decharger")]
    private GameObject m_currentLevel;
    
    [Header("Transform")]
    [SerializeField, Tooltip("Position de l'assensceur actuel")]
    private GameObject m_colliderBlock;
    
    [SerializeField, Tooltip("Position de l'assensceur actuel")]
    private Transform m_currentPosElevator;
    
    [SerializeField, Tooltip("Autre Assensceur qui va recevoir le joueur")]
    private Transform m_newPosElevator;
    
    [Header("Sound Emitter")]
    [SerializeField, Tooltip("Emitter d'ouverture de porte")]
    private StudioEventEmitter m_openEmitter;
    
    [SerializeField, Tooltip("Emitter de fermeture de porte")]
    private StudioEventEmitter m_closeEmitter;
    
    [Header("Animator")]
    [SerializeField, Tooltip("Liste des animator des portes")]
    private List<Animator> m_listAnimator;
    
    [SerializeField, Tooltip("Est ce que l'elevator tp le joueur")]
    private bool m_isTeleport;

    private int m_openHash;
    private int m_closeHash;
    
    [SerializeField, Tooltip("Checked: le trigger ouvre les portes")]
    private bool m_isOpen;

    private Coroutine m_coroutineClose;

    private void Awake()
    {
        m_openHash = Animator.StringToHash("Open");
        m_closeHash = Animator.StringToHash("Close");
        
        if(m_nextLevel)m_nextLevel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        OpenDoorElevator();
        
    }

    private void OpenDoorElevator()
    {
        if (m_isOpen)
        {
            ResetTrigger(m_closeHash);
            SetTrigger(m_openHash);
            if(m_openEmitter != null) m_openEmitter.Play();
            gameObject.SetActive(false);
            return;
        }

        if(m_closeEmitter != null) m_closeEmitter.Play();
        
        ResetTrigger(m_openHash);
        SetTrigger(m_closeHash);

        if (!m_isTeleport)
        {
            m_colliderBlock.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        StartCoroutine(WaitBeforeTP());
    }

    private void ResetTrigger(int Hash)
    {
        foreach (Animator anim in m_listAnimator)
        {
            anim.ResetTrigger(Hash);
        }
    }

    private void SetTrigger(int Hash)
    {
        foreach (Animator anim in m_listAnimator)
        {
            anim.SetTrigger(Hash);
        }
    }

    IEnumerator WaitBeforeTP()
    {
        yield return new WaitForSeconds(6);
        
        m_nextLevel.SetActive(true);
        m_currentLevel.SetActive(false);
        
        PlayerManager.Instance.m_controllerScript.m_charaController.enabled = false;
        PlayerManager.Instance.transform.position =  m_newPosElevator.position + (PlayerManager.Instance.transform.position - m_currentPosElevator.position);
        
        
        yield return new WaitForEndOfFrame();
        PlayerManager.Instance.m_controllerScript.m_charaController.enabled = true;
        
        
        gameObject.SetActive(false);
    }
}
