using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SaveSystem.SavePlayer(PlayerManager.Instance);
        
        PlayerManager.Instance.SetCheckPoint(PlayerManager.Instance.transform.position);
        gameObject.SetActive(false);
    }
}
