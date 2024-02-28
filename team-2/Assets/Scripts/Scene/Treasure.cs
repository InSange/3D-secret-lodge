using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Scene
{
    [SerializeField] GameObject map;
    // Start is called before the first frame update
    void Start()
    {
        LoadMapData();
        LoadPlayer("Treasure_SpawnPoint");
        LoadFinish();
    }
    private void LoadMapData()
    {
        map = Instantiate((GameObject)Resources.Load("Scene/Treasure/Treasure"));
        map.transform.SetParent(this.transform);
        room = GetComponentInChildren<RoomData>();
        room.SetSceneData(this);
        room.RoomSetting();
        room.artifact.playerGetArtifact += TreasureSecondPhase;
    }

    public void TreasureSecondPhase()
    {
        room.door.SetDoorType(DoorType.door);
    }

    protected override void Update()
    {
        base.Update();
    }

}
