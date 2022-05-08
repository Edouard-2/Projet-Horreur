[System.Serializable]
public class PlayerDataSave
{
    public float[] position = new float[3];

    public PlayerDataSave(PlayerManager player)
    {
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}

[System.Serializable]
public class ActiveSave
{
    public bool m_active;

    public ActiveSave(bool p_bool)
    {
        m_active = p_bool;
    }
}