using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public delegate void DisplayUIGamePause(bool p_active = true);
    public DisplayUIGamePause DoDisplayUIGamePause;
    
    public delegate void DisplayUIMainMenu(bool p_active = true);
    public DisplayUIMainMenu DoDisplayUIMainMenu;

    public bool m_isOption = false;
    
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
    
    private void ActivePauseUI(int p_isActive = 1)
    {
        if (p_isActive == 0)
        {
            
            Cursor.lockState = CursorLockMode.Locked;
            DoDisplayUIGamePause?.Invoke(false);
            
            //Enlever l'UI du menu pause
            Debug.Log("Remise en place du jeu");
        }
        else if (p_isActive == 1)
        {
            
            DoDisplayUIGamePause?.Invoke();
            Cursor.lockState = CursorLockMode.Confined;
            
            //Activer l'ui du menu pause
            Debug.Log("Activation de l'UI");
        }
    }

    protected override string GetSingletonName()
    {
        return "UIManager";
    }
}
