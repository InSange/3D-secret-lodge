using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : Scene
{
    [SerializeField] GameObject map;
    [SerializeField] List<Transform> spawnPoints;

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
        room.artifact.playerGetArtifact += MazeSecondPhase;
    }

    public void MazeSecondPhase()
    {
        room.door.SetDoorType(DoorType.door);

        for (int i = 0; i < room.monsters.Count; i++)
        {
            room.monsters[i].detectCollider.radius = 300.0f;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
