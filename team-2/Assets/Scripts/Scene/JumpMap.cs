using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMap : Scene
{    /// <summary>
    ///  Jump맵 씬을 세팅하기 위한 작업
    /// </summary>
    public override void load()
    {
        base.load();
        LoadMapData();
        LoadPlayer("JumpMap_SpawnPoint");
        LoadFinish();
    }
    /// <summary>
    /// 점프맵을 구성하는 배경과 룸 데이터 세팅.
    /// </summary>
    private void LoadMapData()
    {
        map = Instantiate((GameObject)Resources.Load("Scene/JumpMap/JumpMap"));
        map.transform.SetParent(this.transform);
        room = GetComponentInChildren<RoomData>();
        room.SetSceneData(this);
        room.RoomSetting();
    }
}
