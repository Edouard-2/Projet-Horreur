using System.Collections.Generic;
using UnityEngine;

public interface IDoor
{
    void OpenDoor(List<KeyType> p_playerKeys, GameObject p_door);

}
