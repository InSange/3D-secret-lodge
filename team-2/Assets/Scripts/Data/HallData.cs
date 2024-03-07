using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallData : RoomData
{
    [SerializeField] GameObject jumpMapArtifact;
    [SerializeField] GameObject mazeArtifact;
    [SerializeField] GameObject treasureArtifact;
    [SerializeField] GameObject trapArtifact;

    // Door;
    [SerializeField] Door jumpMap;
    [SerializeField] Door maze;
    [SerializeField] Door trap;
    [SerializeField] Door treasure;

    public override void RoomSetting()
    {
        if (GameManager.data.clearJumpMap) jumpMapArtifact.SetActive(true);
        if (GameManager.data.clearMaze) mazeArtifact.SetActive(true);
        if (GameManager.data.clearTreasure) treasureArtifact.SetActive(true);
        if (GameManager.data.clearTrap) trapArtifact.SetActive(true);

        jumpMap.SetDoorNextScene(SceneName.JumpMap);
        maze.SetDoorNextScene(SceneName.Maze);
        trap.SetDoorNextScene(SceneName.Trap);
        treasure.SetDoorNextScene(SceneName.Treasure);

        if (GameManager.data.visitedHall == false) NeedTalkForOpenDoor();
        else SettingDoor();

        GameManager.Instance.eventStart += SettingDoor;
    }

    void NeedTalkForOpenDoor()
    {
        Debug.Log("대화가 필요해!");
        jumpMap.SetDoorType(DoorType.need_talk);
        maze.SetDoorType(DoorType.need_talk);
        trap.SetDoorType(DoorType.need_talk);
        treasure.SetDoorType(DoorType.need_talk);
    }

    void SettingDoor()
    {
        Debug.Log("다른 방으로 이동하자!");
        GameManager.Instance.eventStart -= SettingDoor;
        jumpMap.SetDoorType(GameManager.data.clearJumpMap ? DoorType.clear : DoorType.door);
        maze.SetDoorType(GameManager.data.clearMaze ? DoorType.clear : DoorType.door);
        trap.SetDoorType(GameManager.data.clearTrap ? DoorType.clear : DoorType.door);
        treasure.SetDoorType(GameManager.data.clearTreasure ? DoorType.clear : DoorType.door);
    }
}
