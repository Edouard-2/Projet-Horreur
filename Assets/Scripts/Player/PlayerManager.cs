using UnityEngine;

[RequireComponent(typeof(PlayerLook)), RequireComponent(typeof(PlayerVision)),RequireComponent(typeof(PlayerController))]
public class PlayerManager : Singleton<PlayerManager>
{
    //Constante
    private const float m_gravity = -9.81f;

    public float Gravity
    {
        get => m_gravity;
    }

    public delegate void DoMouvment();
    public DoMouvment DoMouvmentHandler;
    public DoMouvment DoCursorHandler;
    public DoMouvment DoSwitchViewHandler;

    public delegate void DoVisionSwitch();
    public DoVisionSwitch DoVisibleToInvisibleHandler;

    private void Update()
    {
        //Mouvement du Joueur
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            DoMouvmentHandler?.Invoke();
        }

        //Mouvement de la camera
        DoCursorHandler?.Invoke();
        
        //Input du changement de vision
        DoSwitchViewHandler?.Invoke();
    }

    public void LaunchDelegate()
    {
        DoVisibleToInvisibleHandler?.Invoke();
    }

    protected override string GetSingletonName()
    {
        return "PlayerManager";
    }
}