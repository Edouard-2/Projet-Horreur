using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerManager.Instance.SetCheckPoint(PlayerManager.Instance.transform.position);
        
        gameObject.SetActive(false);
    }
}
