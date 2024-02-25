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
        map = Instantiate((GameObject)Resources.Load("Scene/Maze/Maze"));
        map.transform.SetParent(this.transform);

        door = GameObject.FindWithTag("Door").GetComponent<Door>();
        door.SetDoorNextScene(SceneName.Hall);
        door.SetDoorType(DoorType.broken_door);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
