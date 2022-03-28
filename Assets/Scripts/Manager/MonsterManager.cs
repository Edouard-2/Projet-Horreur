using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    public void StartIA()
    {
        Debug.Log("Activation monstre");
    }

    public void EndIA()
    {
        Debug.Log("Désactivation monstre");
    }
    
    protected override string GetSingletonName()
    {
        return "MonsterManager";
    }
}