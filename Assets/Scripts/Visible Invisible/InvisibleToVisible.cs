using UnityEngine;

public class InvisibleToVisible : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerManager.Instance.DoVisibleToInvisibleHandler += DoInvisibleToVisible;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.DoVisibleToInvisibleHandler -= DoInvisibleToVisible;
    }

    void DoInvisibleToVisible(bool p_start)
    {
        //Si True
        //  Enlever ombre
        //  Enlever Collider
        //Sinon
        //  Mettre ombre
        //  Mettre Collider
    }
}
