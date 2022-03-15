using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    //Constante
    private const float m_gravity = -9.81f;

    public float Gravity
    {
        get => m_gravity;
    }

    public delegate void DoMouvment();
    public delegate void DoCursorMouvement();
    public delegate void DoSwitchView();

    public DoMouvment DoMouvmentHandler;
    public DoCursorMouvement DoCursorMouvementHandler;
    public DoSwitchView DoSwitchViewHandler;
    private void Update()
    {
        //Mouvement du Joueur
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            DoMouvmentHandler?.Invoke();
        }
        //Mouvement de la camera
        DoCursorMouvementHandler?.Invoke();
        
        //Input du changement de vision
        DoSwitchViewHandler?.Invoke();
    }

    protected override string GetSingletonName()
    {
        return "PlayerManager";
    }
}