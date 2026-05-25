using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InterfaceSavemanager 
{
    void LoadData(Gamedata _data);
    void SaveData(ref Gamedata _data);
}
