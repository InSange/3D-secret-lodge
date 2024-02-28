using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMapData : RoomData
{
    [SerializeField] List<Transform> monster_spawn;
    [SerializeField] List<Monster> monsters;
    // Start is called before the first frame update
    void Start()
    {
        artifact.playerGetArtifact += MazeSecondPhase;
    }

    public override void RoomSetting()
    {
        base.RoomSetting();
    }

    public void MazeSecondPhase()
    {
        door.SetDoorType(DoorType.door);

        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].detectCollider.radius = 300.0f;
        }
    }
}
