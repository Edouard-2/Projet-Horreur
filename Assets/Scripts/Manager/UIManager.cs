using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public delegate void DisplayUIGamePause(bool p_active = true);
    public DisplayUIGamePause DoDisplayUIGamePause;
    
    [HideInInspector]
    public bool m_isOption;
    
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
            PlayerManager.Instance.m_textState.text = "zirjBGNIOJEnrginJERGPNezrgnZERJNGzirjBGNIOJEnrginJERGPNezrgnZERJNG ";
            //Enlever l'UI du menu pause
            Debug.Log("Remise en place du jeu");
        }
        else if (p_isActive == 1)
        {
            DoDisplayUIGamePause?.Invoke();
            Cursor.lockState = CursorLockMode.Confined;
            PlayerManager.Instance.m_textState.text = "zirjBGNIOJEnrginJERGPNezrgnZERJNG";
            //Activer l'ui du menu pause
            Debug.Log("Activation de l'UI");
        }
    }

    protected override string GetSingletonName()
    {
        return "UIManager";
    }
}
