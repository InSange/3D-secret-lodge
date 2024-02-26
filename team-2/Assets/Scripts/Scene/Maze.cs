using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : Scene
{
    [SerializeField] GameObject map;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] Door door;

    // Start is called before the first frame update
    void Start()
    {
        LoadMapData();

        LoadPlayer("Maze_SpawnPoint");
        LoadFinish();
    }

    private void LoadMapData()
    {
        transform.position = new Vector3(3000.0f, 0, 0);
        map = Instantiate((GameObject)Resources.Load("Scene/Maze/Maze"));
        map.transform.SetParent(this.transform);
        room = GetComponentInChildren<RoomData>();
        room.SetSceneData(this);
        room.RoomSetting();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
