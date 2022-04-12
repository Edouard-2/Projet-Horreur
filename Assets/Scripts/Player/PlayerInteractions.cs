using System.Collections;
using System.Net.Mime;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    //LayerMask Visible
    [Header("Layer Neutral")]
    [SerializeField, Tooltip("Layer pour les door")] private LayerMask m_layerDoor;
    [SerializeField, Tooltip("Layer pour les key")] public LayerMask m_layerKey;
    [SerializeField, Tooltip("Layer pour les transvaseur")] private LayerMask m_layerTransvaseur;
    
    //LayerMaske Invisible To Visible
    [Header("Layer Invisinble")]
    [SerializeField, Tooltip("Layer pour les doorInvisible")] private LayerMask m_layerDoorInvisible;
    [SerializeField, Tooltip("Layer pour les keyInvisibleToVisible")] public LayerMask m_layerKeyInvisible;
    [SerializeField, Tooltip("Layer pour les transvaseurInvisible")] public LayerMask m_layerTransvaseurInvisible;
    
    //LayerMaske Visible To Invisible
    [Header("Layer Visible")]
    [SerializeField, Tooltip("Layer pour les keyVisibleToInvisible")] public LayerMask m_layerKeyVisible;
    [SerializeField, Tooltip("Layer pour les doorVisibleToInvisible")] public LayerMask m_layerDoorVisible;

    //Layer raycast de proximité
    [Header("Layer Raycast")]
    [SerializeField, Tooltip("Les layers qui seront choisis pour la détection du raycast de proximité")] public LayerMask m_targetLayer;
    
    //Objects
    [Header("Other")]
    [SerializeField, Tooltip("Trousseau de clé")] public KeyType m_trousseauKey;
    [SerializeField, Tooltip("La piscine de toutes les clés")] private Transform m_pool;
    [SerializeField, Tooltip("Feedback visuel Cursor")] private GameObject m_cursorSelection;
    [SerializeField, Tooltip("Feedback visuel Text nom obj")] private TextMeshProUGUI m_textSelection;
    [SerializeField, Tooltip("Mesh render de l'obj key UI")] private MeshRenderer m_keyUiRenderer;
    [SerializeField, Tooltip("Animator de l'obj key UI")] private Animator m_keyUiAnimator;
    [SerializeField, Tooltip("Clé in game actuel")] public GameObject m_keyObject;

    private int m_triggerEnter;
    private int m_triggerExit;
    
    [SerializeField, Tooltip("Temps entre l'animation de sortie de la carte et d'entré")] private float m_waitForAnimationCompleteKeyValue;
    
    private WaitForSeconds m_waitForAnimationCompleteKey;
    private Material m_currentAimObject;

    private bool m_isVariablesReady = true;

    private void Awake()
    {
        m_waitForAnimationCompleteKey = new WaitForSeconds(m_waitForAnimationCompleteKeyValue);

        m_triggerEnter = Animator.StringToHash("Enter");
        m_triggerExit = Animator.StringToHash("Exit");
        
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
            targetMaterial = p_target.GetComponent<Key>().m_key.m_keyMat;

        }
        
        //Récupérer le mat de la porte
        else if ((m_layerDoor.value & (1 << p_target.gameObject.layer)) > 0 || 
                 (m_layerDoorInvisible.value & (1 << p_target.gameObject.layer)) > 0 ||
                 (m_layerDoorVisible.value & (1 << p_target.gameObject.layer)) > 0)
        {
            //Debug.Log("porte");
            targetMaterial = p_target.GetComponent<Door>().m_doorMat;
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
                Key myKey = p_target.GetComponent<Key>();
                if (myKey && myKey.OpenChest(out KeyType key))
                {
                    if (m_keyObject == null)
                    {
                        
                        m_trousseauKey = key;
                        m_keyObject = p_target.gameObject;
                        SetUIKey(myKey);
                    }
                    else
                    {
                        
                        //Enlever la précedente clé
                        EjectKey();
                        
                        m_trousseauKey = key;
                        m_keyObject = p_target.gameObject;
                        SetUIKey(myKey);
                    }
                }
            }

            //Si c'est la porte
            else if ((m_layerDoor.value & (1 << p_target.gameObject.layer)) > 0 ||
                     (m_layerDoorInvisible.value & (1 << p_target.gameObject.layer)) > 0 ||
                     (m_layerDoorVisible.value & (1 << p_target.gameObject.layer)) > 0)
            {
                Debug.Log("Door ");
                Door myDoor = p_target.GetComponent<Door>();
                if (myDoor)
                {
                    if (myDoor.OpenDoor(m_trousseauKey))
                    {
                        if (myDoor.m_neededKey)
                        {
                            m_trousseauKey = null;
                            StartCoroutine(m_keyObject.GetComponent<Key>().DestroySelf());
                        }
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
                    if (m_trousseauKey != null)
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

    private void SetUIKey(Key p_key, bool p_isSet = false)
    {
        if (m_isVariablesReady)
        {
            if (m_trousseauKey != null && !p_isSet)
            {
                //Lancer la coroutine de sortie
                m_keyUiAnimator.ResetTrigger(m_triggerEnter);
                m_keyUiAnimator.SetTrigger(m_triggerExit);
                StartCoroutine(WaitUntilSetUiKey(p_key, true));
                return;
            }

            m_trousseauKey = p_key.m_key;
            //Mettre le bon matérial
            m_keyUiRenderer.material = p_key.m_key.m_keyMat;
            
            Material temp = m_keyUiRenderer.gameObject.GetComponent<MeshRenderer>().material;
            
            temp.SetFloat("_isAim", 0);
            
            //Mettre la carte ingame dans la pool
            p_key.transform.SetParent(m_pool);
            p_key.transform.localPosition = Vector3.zero;
            
            //Lancer l'anim d'arrivé
            m_keyUiAnimator.ResetTrigger(m_triggerExit);
            m_keyUiAnimator.SetTrigger(m_triggerEnter);
            
            Debug.Log(p_key.m_key.m_keyMat);
        }
    }

    IEnumerator WaitUntilSetUiKey(Key p_key,bool p_isSet = false)
    {
        yield return m_waitForAnimationCompleteKey;
        SetUIKey(p_key, p_isSet);
    }
    
    public void EjectKey(bool p_position = true)
    {
        if (m_isVariablesReady && m_keyObject!= null)
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
            
            //Lancer l'animation de sorite
            m_keyUiAnimator.ResetTrigger(m_triggerEnter);
            m_keyUiAnimator.SetTrigger(m_triggerExit);
        }
    }
}