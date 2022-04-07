using UnityEngine;

[RequireComponent(typeof(UIMainMenu_Over))]
public class UIMainMenu_Exit : MonoBehaviour
{
    private void OnMouseDown()
    {
        Application.Quit();
    }
}
