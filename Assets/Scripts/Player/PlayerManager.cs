using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerLook))]
[RequireComponent(typeof(PlayerVision))]
[RequireComponent(typeof(PlayerInteractions))]
public class PlayerManager : Singleton<PlayerManager>
{
    //Constante
    private const float m_gravity = -9.81f;

    //Camera
    [SerializeField, Tooltip("Camera Principale")] public Camera m_camera;
    
    //LayerMask
    [SerializeField, Tooltip("Layer pour les keyInvisible")] private LayerMask m_keyLayerInvisible;
    [SerializeField, Tooltip("Layer pour les doorInvisible")] private LayerMask m_doorLayerInvisible;
    [SerializeField, Tooltip("Layer pour les transvaseurInvisibles")] private LayerMask m_transvaseurLayerInvisible;
    
    //Scripts
    [SerializeField, Tooltip("Script player controller")] private PlayerController m_controllerScript;
    [SerializeField, Tooltip("Script player look")] private PlayerLook m_lookScript;
    [SerializeField, Tooltip("Script player vision")] public PlayerVision m_visionScript;
    [SerializeField, Tooltip("Script player door")] private PlayerInteractions m_interactionsScript;

    public float m_radiusVision;
    
    private float m_timeVision;
    private float tTime;
    public float Gravity
    {
        get => m_gravity;
    }

    public delegate void RotateKeys();
    public RotateKeys DoRotateKeys;
    public delegate void DoVisionSwitch();
    public DoVisionSwitch DoVisibleToInvisibleHandler;
    
    private void Awake()
    {
        GameManager.Instance.SetState(GameManager.States.PLAYING);
        
        Cursor.lockState = CursorLockMode.Locked;
        
        m_visionScript.m_matVision.SetFloat("_BlurSize",0);
        m_visionScript.m_matInvisibleVisible.SetFloat("_StepStrenght",-0.03f);
        m_visionScript.m_matVisibleInvisible.SetFloat("_StepStrenght",-0.03f);

        if (m_controllerScript == null)
            m_controllerScript = GetComponent<PlayerController>();

        if (m_lookScript == null)
            m_lookScript = GetComponent<PlayerLook>();

        if (m_visionScript == null)
            m_visionScript = GetComponent<PlayerVision>();
        
        if (m_interactionsScript == null)
            m_interactionsScript = GetComponent<PlayerInteractions>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.SwitchPauseGame();
        }
        
        if (GameManager.Instance.State == GameManager.States.PLAYING)
        {
            //Mouvement du Joueur
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                m_controllerScript.Mouvement();
            }
        
            //Interaction avec des objets
            if (Input.GetKeyDown(KeyCode.E))
            {
                UpdateCheckInteraction();
            }

            //Mettre le jeu en pause

            UpdateCheckFeedbackInteract();
        
            IsInputDown();
            
            m_lookScript.CursorMouvement();
            
            DoRotateKeys?.Invoke();
        }
    }

    private void UpdateCheckFeedbackInteract()
    {
        RaycastHit hitInteract;
        Ray rayInteract = m_camera.ScreenPointToRay(Input.mousePosition);
        
        //Changement de materiaux si l'obj est interactif et visé par le joueur
        if (Physics.Raycast(rayInteract, out hitInteract, m_radiusVision))
        { 
            //Verification si le joueur est en vision flou
            if (m_visionScript.m_readyEnd == 0)
            {
                //Si oui est ce que l'obj est visible (net) en mode flou
                if((m_keyLayerInvisible.value & (1 << hitInteract.transform.gameObject.layer)) > 0 
                   || (m_doorLayerInvisible.value & (1 << hitInteract.transform.gameObject.layer)) > 0 
                   || (m_transvaseurLayerInvisible.value & (1 << hitInteract.transform.gameObject.layer)) > 0)
                {
                    m_interactionsScript.VerifyFeedbackInteract(hitInteract.transform);
                }
            }
            else
            {
                m_interactionsScript.VerifyFeedbackInteract(hitInteract.transform);
            }
        }
        //Sinon on remet le materiaux de base
        else
        {
            m_interactionsScript.ResetFeedbackInteract();
        }
    }
    
    private void UpdateCheckInteraction()
    {
        RaycastHit hit;
        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, m_radiusVision))
        {
            //Verification si le joueur est en vision flou
            if (m_visionScript.m_readyEnd == 0)
            {
                //Si oui est ce que l'obj est visible (net) en mode flou
                if((m_keyLayerInvisible.value & (1 << hit.transform.gameObject.layer)) > 0 
                   || (m_doorLayerInvisible.value & (1 << hit.transform.gameObject.layer)) > 0 
                   || (m_transvaseurLayerInvisible.value & (1 << hit.transform.gameObject.layer)) > 0)
                {
                    m_interactionsScript.VerifyLayer(hit.transform);
                }
            }
            else
            {
                m_interactionsScript.VerifyLayer(hit.transform);
            }
        }
    }
    
    public void InitVariableChangement()
    {
        if (m_visionScript.m_readyInitVision)
        {
            Debug.Log("Init variable pour vision");
            m_timeVision = Time.time;
            tTime = Time.time - m_timeVision;

            m_visionScript.m_readyEnd = Mathf.Abs(m_visionScript.m_readyEnd - 1);

            m_visionScript.m_resetTimeVisionComp = true;
            m_visionScript.m_resetTimeVisionMat = true;
            
            DoVisibleToInvisibleHandler?.Invoke();
            
            if (m_interactionsScript.m_keyObject != null)
            {
                CheckCurrentKey(m_visionScript.m_readyEnd);
            }
        }
    }

    public void IsInputDown()
    {
        tTime = Time.time - m_timeVision;

        //Changement de vision
        if (Input.GetKeyDown(KeyCode.Space) && !m_visionScript.m_resetTimeVisionComp && !m_visionScript.m_resetTimeVisionMat )
        {

            InitVariableChangement();

            Debug.Log(m_visionScript.m_readyEnd);

            if (m_visionScript.m_readyEnd == 0)
            {
                Debug.Log("hey");
                //Ajout du cran sur la BV
                m_visionScript.AddStepBV();
            }

        }

        if (m_visionScript.m_readyEnd == 0)
        {
            if (m_visionScript.m_resetTimeVisionComp)
            {
                //Debug.Log("Start");

                //DoSwitchView(allé)
                m_visionScript.DoSwitchView(tTime, m_visionScript.m_curveVisionStart);
            }
            if (m_visionScript.m_resetTimeVisionMat)
            {
                //DoSwitchMaterial(allé)
                m_visionScript.DoSwitchMaterial(tTime, m_visionScript.m_curveMatVisionStart);
            }

            //Lancement de la consommation de BV
            m_visionScript.DecreaseBV();
        }

        else if (m_visionScript.m_readyEnd == 1)
        {
            if (m_visionScript.m_resetTimeVisionComp)
            {
                //Debug.Log("Fin");
                //DoSwitchView(retour)
                m_visionScript.DoSwitchView(tTime, m_visionScript.m_curveVisionFinish);
            }
            if (m_visionScript.m_resetTimeVisionMat)
            {
                //DoSwitchMaterial(retour)
                m_visionScript.DoSwitchMaterial(tTime, m_visionScript.m_curveMatVisionFinish);
            }
            m_visionScript.IncreaseBV();
            
            
        }
    }

    public void CheckCurrentKey(int p_readyEnd)
    {
        if (m_interactionsScript.m_trousseauKey != null)
        {
            if (p_readyEnd == 0 )
            {
                if((m_interactionsScript.m_layerKeyVisible & (1 << m_interactionsScript.m_keyObject.layer)) > 0)
                {
                    Debug.Log("Jeter la clé");
                    m_interactionsScript.EjectKey();
                }
            }
            else if (p_readyEnd == 1 )
            {
                if((m_interactionsScript.m_layerKeyInvisible & (1 << m_interactionsScript.m_keyObject.layer)) > 0)
                {
                    Debug.Log("Jeter la clé");
                    m_interactionsScript.EjectKey();
                }
            }
        }
        
    }

    protected override string GetSingletonName()
    {
        return "PlayerManager";
    }
}