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
    [SerializeField] Door boss;

    // NPC
    [SerializeField] NPC boy;
    [SerializeField] NPC man;
    [SerializeField] NPC robin;
    [SerializeField] NPC dwarf;
    [SerializeField] NPC woman;

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

        if(GameManager.data.clearJumpMap && GameManager.data.clearMaze
            && GameManager.data.clearTrap && GameManager.data.clearTreasure)
        {
            boy.gameObject.SetActive(false);
            man.gameObject.SetActive(false);
            robin.gameObject.SetActive(false);
            dwarf.gameObject.SetActive(false);
            woman.gameObject.SetActive(false);
            boss.SetDoorType(DoorType.door);
        }
        else
        {
            boy.SetType(NPC_TYPE.Boy);
            man.SetType(NPC_TYPE.Man);
            robin.SetType(NPC_TYPE.Robin);
            dwarf.SetType(NPC_TYPE.Dwarf);
            woman.SetType(NPC_TYPE.Woman);
        }

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
