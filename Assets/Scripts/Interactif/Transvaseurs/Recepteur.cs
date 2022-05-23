using System.Collections;
using FMODUnity;
using UnityEngine;

public class Recepteur : MonoBehaviour
{
    [SerializeField, Tooltip("Material global du transvaseur")]
    public Material m_material;
    
    [SerializeField, Tooltip("Material global du transvaseur")]
    public EventsTriggerPos m_soundAlert;
    
    [SerializeField, Tooltip("L'autre Recepteur du Transvaseur")]
    private Recepteur m_otherRecepeteur;
    
    [SerializeField, Tooltip("L'endroit ou l'obj va arriver depuis l'autre recepteur")]
    private Transform m_spawnObjects;
    
    [SerializeField, Tooltip("Sound emitter qui va faire le transvaseur")]
    public StudioEventEmitter m_transvaseurEmitter;
    
    [SerializeField, Tooltip("Sound emitter pour le voyage du colis dans les tubes")]
    public StudioEventEmitter m_travelEmitter;

    private WaitForSeconds m_waitRecepteurTravel = new WaitForSeconds(4f);

    private void Awake()
    {
        if (m_spawnObjects == null)
        {
            m_spawnObjects = transform.GetChild(0);
            if (m_spawnObjects == null)
            {
                Debug.LogError("Il faut mettre un Spawn pour le recepteur (mettre un empty enfant du Recepteur)", this);
            }
        }
        
        m_material.SetFloat("_isAim",0);
    }

    //Fonction du Transvaseur
    public void TeleportObject(Transform p_target)
    {
        GetComponent<BoxCollider>().enabled = false;
        m_soundAlert.Raise(PlayerManager.Instance.transform.position);
        m_transvaseurEmitter.Play();
        m_travelEmitter.Play();
        p_target.position = m_otherRecepeteur.m_spawnObjects.position;
        StartCoroutine(EmitterRecepteur());
    }

    IEnumerator EmitterRecepteur()
    {
        yield return m_waitRecepteurTravel;
        m_otherRecepeteur.m_travelEmitter.Play();
        yield return m_waitRecepteurTravel;
        m_otherRecepeteur.m_transvaseurEmitter.Play();
    }
}