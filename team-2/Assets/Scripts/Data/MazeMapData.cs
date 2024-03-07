using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMapData : RoomData
{
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
        door.doorEvent += OutHall;

        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].detectCollider.radius = 300.0f;
        }
    }

    public override void OutHall()
    {
        base.OutHall();
        GameManager.data.clearMaze= true;
        GameManager.SaveGameData();
        door.doorEvent -= OutHall;
        Debug.Log("미로 세이브 완료!");
    }
}
