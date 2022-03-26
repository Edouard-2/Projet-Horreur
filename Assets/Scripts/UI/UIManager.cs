using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// Enum state : playing, pause, loading, main menu
    /// player update => if(playing)
    /// monstre update => if(playing)
    /// if(press pause) => gamemanager.state = pause - UIManager.afficher menu pause / state = main menu - UIManager.afficher main menu
    ///
    /// Mettre tout les boutons UI dans le bon déléguate 
    /// </summary>

    public delegate void DisplayUIGamePause(bool p_active = true);
    public DisplayUIGamePause DoDisplayUIGamePause;
    
    public delegate void DisplayUIMainMenu(bool p_active = true);
    public DisplayUIMainMenu DoDisplayUIMainMenu;
    
    private void OnEnable()
    {
        GameManager.Instance.DoUiActivePauseGame += ActivePauseUI;
    }
    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.DoUiActivePauseGame -= ActivePauseUI;
    }

    private void Awake()
    {
        DoDisplayUIGamePause?.Invoke(false);
    }
    
    private void ActivePauseUI(bool p_isActive = true)
    {
        DoDisplayUIGamePause?.Invoke(p_isActive);
        
        if (p_isActive)
        {
            Cursor.lockState = CursorLockMode.Confined;
            
            //Activer l'ui du menu pause
            Debug.Log("Activation de l'UI");
            
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        
        //Enlever l'UI du menu pause
        Debug.Log("Remise en place du jeu");
    }

    protected override string GetSingletonName()
    {
        return "UIManager";
    }
}
