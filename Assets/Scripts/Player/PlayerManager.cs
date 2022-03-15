using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    //Constante
    private const float m_gravity = -9.81f;

    public float Gravity
    {
        get => m_gravity;
    }

    public delegate void DelegateEvent();

    public DelegateEvent DoMouvment;
    public DelegateEvent DoCursorMouvement;
    public DelegateEvent DoSwitchView;
    private void Update()
    {
        //Mouvement du Joueur
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            DoMouvment?.Invoke();
        //Mouvement de la camera
        DoCursorMouvement?.Invoke();
        
        //Input du changement de vision
        DoSwitchView?.Invoke();
    }

    protected override string GetSingletonName()
    {
        return "PlayerManager";
    }
}