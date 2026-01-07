using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Scene
{/// <summary>
/// 함정 씬 세팅해준다.
/// </summary>
    public override void load()
    {
        base.load();

        LoadMapData();
        LoadPlayer("TrapRoom SpawnPoint");
        LoadFinish();
    }
    /// <summary>
    /// 씬을 구성하는 배경과 룸 데이터 세팅
    /// </summary>
    private void LoadMapData()
    {
        map = Instantiate((GameObject)Resources.Load("Scene/Trap/Trap_Room"));
        map.transform.SetParent(this.transform);
        room = GetComponentInChildren<RoomData>();
        room.SetSceneData(this);
        room.RoomSetting();
    }
}
