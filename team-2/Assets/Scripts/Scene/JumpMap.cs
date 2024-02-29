using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMap : Scene
{
    [SerializeField] GameObject map;
    // Start is called before the first frame update
    void Start()
    {
        LoadMapData();
        LoadPlayer("JumpMap_SpawnPoint");
        LoadFinish();
    }

    private void LoadMapData()
    {
        map = Instantiate((GameObject)Resources.Load("Scene/JumpMap/JumpMap"));
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
