using UnityEngine;

public class Recepteur : MonoBehaviour
{
    [SerializeField, Tooltip("Material global du transvaseur")]
    public Material m_material;
    
    [SerializeField, Tooltip("L'autre Recepteur du Transvaseur")]
    private Recepteur m_otherRecepeteur;
    
    [SerializeField, Tooltip("L'endroit ou l'obj va arriver depuis l'autre recepteur")]
    private Transform m_spawnObjects;

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
        p_target.position = m_otherRecepeteur.m_spawnObjects.position;
    }
}