using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : Scene
{/// <summary>
/// 미로 씬 세팅해준다.
/// </summary>
    public override void load()
    {
        base.load();

        LoadMapData();
        LoadPlayer("Maze_SpawnPoint");
        LoadFinish();
    }
    /// <summary>
    /// 씬을 구성하는 배경과 룸 데이터 세팅
    /// </summary>
    private void LoadMapData()
    {
        transform.position = new Vector3(3000.0f, 0, 0);
        map = Instantiate((GameObject)Resources.Load("Scene/Maze/Maze"));
        map.transform.SetParent(this.transform);
        room = GetComponentInChildren<RoomData>();
        room.SetSceneData(this);
        room.RoomSetting();
    }
}
