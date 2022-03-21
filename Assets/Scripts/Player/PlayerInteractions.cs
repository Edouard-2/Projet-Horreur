using UnityEngine.UI;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    //LayerMask Visible
    [SerializeField, Tooltip("Layer pour les door")] private LayerMask m_layerDoor;
    [SerializeField, Tooltip("Layer pour les key")] private LayerMask m_layerKey;
    [SerializeField, Tooltip("Layer pour les transvaseur")] private LayerMask m_layerTransvaseur;
    
    //LayerMaske Invisible
    [SerializeField, Tooltip("Layer pour les doorInvisible")] private LayerMask m_layerDoorInvisible;
    [SerializeField, Tooltip("Layer pour les keyInvisible")] public LayerMask m_layerKeyInvisible;
    [SerializeField, Tooltip("Layer pour les transvaseurInvisible")] public LayerMask m_layerTransvaseurInvisible;

    [SerializeField, Tooltip("Trousseau de clé")] public KeyType m_trousseauKey;
    [SerializeField, Tooltip("UI de la clés ingame")] private Image m_KeyUI;

    public GameObject m_keyObject;
    private Material m_currentAimObject;

    public void VerifyFeedbackInteract(Transform p_target)
    {
        Material targetMaterial = null;
        //Recupérer le mat de la clé
        if ((m_layerKey.value & (1 << p_target.gameObject.layer)) > 0 || (m_layerKeyInvisible.value & (1 << p_target.gameObject.layer)) > 0)
        {
            //Debug.Log("key");
            targetMaterial = p_target.GetComponent<LootBox>().m_key.m_keyMat;

        }
        //Récupérer le mat de la porte
        else if ((m_layerDoor.value & (1 << p_target.gameObject.layer)) > 0 || (m_layerDoorInvisible.value & (1 << p_target.gameObject.layer)) > 0)
        {
            //Debug.Log("porte");
            targetMaterial = p_target.GetComponent<Door>().m_neededKey.m_doorMat;

        }

        if (m_currentAimObject != null && m_currentAimObject != targetMaterial)
        {
            ResetFeedbackInteract();
        }
        
        //Changer le matérial choisi
        if (targetMaterial != null && targetMaterial.GetFloat("_isAim") != 1)
        {
            m_currentAimObject = targetMaterial;
            targetMaterial.SetFloat("_isAim", 1);
        }
    }

    public void ResetFeedbackInteract()
    {
        if(m_currentAimObject != null && m_currentAimObject.GetFloat("_isAim") != 0)
        {
            m_currentAimObject.SetFloat("_isAim", 0);
        }
    }
    
    public void VerifyLayer(Transform p_target)
    {
        //Si c'est la clé
        if ((m_layerKey.value & (1 << p_target.gameObject.layer)) > 0 || 
            (m_layerKeyInvisible.value & (1 << p_target.gameObject.layer)) > 0)
        {
            LootBox myLootBox = p_target.GetComponent<LootBox>();
            if( myLootBox && myLootBox.OpenChest(out KeyType key))
            {
                if (m_trousseauKey == null)
                {
                    m_trousseauKey = key;
                    m_keyObject = p_target.gameObject;
                    SetUIKey(myLootBox);
                }
                else
                {
                    //Enlever la précedente clé
                    EjectKey();
                    m_trousseauKey = key;
                    m_keyObject = p_target.gameObject;
                    SetUIKey(myLootBox);
                }
            }
        }
        
        //Si c'est la porte
        else if ((m_layerDoor.value & (1 << p_target.gameObject.layer)) > 0 || 
                 (m_layerDoorInvisible.value & (1 << p_target.gameObject.layer)) > 0) 
        {
            Door myDoor =  p_target.GetComponent<Door>();
            if (myDoor)
            {
                if (myDoor.OpenDoor(m_trousseauKey, p_target.gameObject))
                {
                    m_trousseauKey = null;
                    StartCoroutine(m_keyObject.GetComponent<LootBox>().DestroySelf());
                    m_KeyUI.color = Color.clear;
                }
            }
        }
        //Si c'est le transvaseur
        else if ((m_layerTransvaseur.value & (1 << p_target.gameObject.layer)) > 0 ||
                  (m_layerTransvaseurInvisible.value & (1 << p_target.gameObject.layer)) > 0)
        {
            
        }
        //Si rien n'est intéractible
        else
        {
            Debug.Log("Rien pour intéragir");
        }
    }

    private void SetUIKey(LootBox p_key)
    {
        Debug.Log(p_key.m_key.m_keyMat);

        m_KeyUI.color = new Vector4(p_key.m_key.m_keyMat.GetColor("_BaseColor").r,p_key.m_key.m_keyMat.GetColor("_BaseColor").g,p_key.m_key.m_keyMat.GetColor("_BaseColor").b,1);
    }
    
    public void EjectKey()
    {
        //Ejecter la clé
        Debug.Log("clear");
        
        m_keyObject.transform.position = transform.position;
        
        RaycastHit hitInteract;
        Ray rayInteract = PlayerManager.Instance.m_camera.ScreenPointToRay(Input.mousePosition);
        
        //Changement de materiaux si l'obj est interactif et visé par le joueur
        if (!Physics.Raycast(rayInteract, out hitInteract, 1))
        {
            m_keyObject.transform.position += transform.forward;
        }
        
        m_trousseauKey = null;
        m_keyObject = null;
        m_KeyUI.color = Color.clear;
    }
}