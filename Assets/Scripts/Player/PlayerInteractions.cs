using System.Net.Mime;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    //LayerMask Visible
    [SerializeField, Tooltip("Layer pour les door")] private LayerMask m_layerDoor;
    [SerializeField, Tooltip("Layer pour les key")] public LayerMask m_layerKey;
    [SerializeField, Tooltip("Layer pour les transvaseur")] private LayerMask m_layerTransvaseur;
    
    //LayerMaske Invisible To Visible
    [SerializeField, Tooltip("Layer pour les doorInvisible")] private LayerMask m_layerDoorInvisible;
    [SerializeField, Tooltip("Layer pour les keyInvisibleToVisible")] public LayerMask m_layerKeyInvisible;
    [SerializeField, Tooltip("Layer pour les transvaseurInvisible")] public LayerMask m_layerTransvaseurInvisible;
    
    //LayerMaske Visible To Invisible
    [SerializeField, Tooltip("Layer pour les keyVisibleToInvisible")] public LayerMask m_layerKeyVisible;

    //Layer raycast de proximité
    [SerializeField, Tooltip("Les layers qui seront choisis pour la détection du raycast de proximité")] public LayerMask m_targetLayer;
    
    //Objects
    [SerializeField, Tooltip("Trousseau de clé")] public KeyType m_trousseauKey;
    [SerializeField, Tooltip("UI de la clés ingame")] private Image m_KeyUI;
    [SerializeField, Tooltip("La piscine de toutes les clés")] private Transform m_pool;
    [SerializeField, Tooltip("Feedback visuel Cursor")] private GameObject m_cursorSelection;
    [SerializeField, Tooltip("Feedback visuel Text nom obj")] private TextMeshProUGUI m_textSelection;

    public GameObject m_keyObject;
    private Material m_currentAimObject;

    private bool m_isVariablesReady = true;

    private void Awake()
    {
        if (m_KeyUI == null)
        {
            Debug.LogError("Il faut mettre la clé UI", this);
            m_isVariablesReady = false;
        }
        if (m_pool == null)
        {
            Debug.LogError("Il faut mettre la Pool", this);
            m_isVariablesReady = false;
        }
    }

    public void VerifyFeedbackInteract(Transform p_target)
    {
        Material targetMaterial = null;
        //Recupérer le mat de la clé
        if ((m_layerKey.value & (1 << p_target.gameObject.layer)) > 0 || 
            (m_layerKeyInvisible.value & (1 << p_target.gameObject.layer)) > 0|| 
            (m_layerKeyVisible.value & (1 << p_target.gameObject.layer)) > 0)
        {
            //Debug.Log("key");
            targetMaterial = p_target.GetComponent<LootBox>().m_key.m_keyMat;

        }
        //Récupérer le mat de la porte
        else if ((m_layerDoor.value & (1 << p_target.gameObject.layer)) > 0 || 
                 (m_layerDoorInvisible.value & (1 << p_target.gameObject.layer)) > 0)
        {
            //Debug.Log("porte");
            targetMaterial = p_target.GetComponent<Door>().m_neededKey.m_doorMat;

        }
        //Récupérer le mat du transvaseur
        else if ((m_layerTransvaseur.value & (1 << p_target.gameObject.layer)) > 0 || 
                 (m_layerTransvaseurInvisible.value & (1 << p_target.gameObject.layer)) > 0)
        {
            
            targetMaterial = p_target.GetComponent<Recepteur>().m_material;
            //Debug.Log(targetMaterial.GetFloat("_isAim"));
        }

        if (m_currentAimObject != null && m_currentAimObject != targetMaterial)
        {
            ResetFeedbackInteract();
        }
        
        //Changer le matérial choisi
        if (targetMaterial != null && targetMaterial.GetFloat("_isAim") != 1)
        {
            Debug.Log("transvaseur");
            //Material
            m_currentAimObject = targetMaterial;
            targetMaterial.SetFloat("_isAim", 1);
            
            //Cursor
            m_textSelection.text = p_target.name;
            m_cursorSelection.SetActive(true);
        }
    }

    public void ResetFeedbackInteract()
    {
        if(m_currentAimObject != null && m_currentAimObject.GetFloat("_isAim") != 0)
        {
            m_cursorSelection.SetActive(false);
            m_currentAimObject.SetFloat("_isAim", 0);
        }
    }
    
    public void VerifyLayer(Transform p_target)
    {
        if (m_isVariablesReady)
        {
            //Si c'est la clé
            if ((m_layerKey.value & (1 << p_target.gameObject.layer)) > 0 ||
                (m_layerKeyInvisible.value & (1 << p_target.gameObject.layer)) > 0 ||
                (m_layerKeyVisible.value & (1 << p_target.gameObject.layer)) > 0)
            {
                LootBox myLootBox = p_target.GetComponent<LootBox>();
                if (myLootBox && myLootBox.OpenChest(out KeyType key))
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
                Door myDoor = p_target.GetComponent<Door>();
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
                Recepteur myRecepteur = p_target.GetComponent<Recepteur>();
                if (myRecepteur)
                {
                    if (m_keyObject != null)
                    {
                        myRecepteur.TeleportObject(m_keyObject.transform);
                        EjectKey(false);
                    }
                    else
                    {
                        Debug.Log("Pas d'objet a transférer");
                    }
                }
            }
            //Si rien n'est intéractible
            else
            {
                Debug.Log("Rien pour intéragir");
            }
        }
    }

    private void SetUIKey(LootBox p_key)
    {
        if (m_isVariablesReady)
        {
            Debug.Log(p_key.m_key.m_keyMat);
            p_key.transform.SetParent(m_pool);
            p_key.transform.localPosition = Vector3.zero;
            
            m_KeyUI.color = new Vector4(p_key.m_key.m_keyMat.GetColor("_BaseColor").r,p_key.m_key.m_keyMat.GetColor("_BaseColor").g,p_key.m_key.m_keyMat.GetColor("_BaseColor").b,1);
        }
    }
    
    public void EjectKey(bool p_position = true)
    {
        if (m_isVariablesReady)
        {
            //Ejecter la clé
            Debug.Log("Clear key");
            m_keyObject.transform.parent = null;
            if (p_position)
            {
                Debug.Log("je reset ma poisition");
                m_keyObject.transform.position = gameObject.transform.position;

                RaycastHit hitInteract;
                Ray rayInteract = PlayerManager.Instance.m_camera.ScreenPointToRay(Input.mousePosition);

                //Changement de materiaux si l'obj est interactif et visé par le joueur
                if (!Physics.Raycast(rayInteract, out hitInteract, 1, m_targetLayer))
                {
                    m_keyObject.transform.position += transform.forward;
                }
            }

            m_trousseauKey = null;
            m_keyObject = null;
            m_KeyUI.color = Color.clear;
        }
    }
}