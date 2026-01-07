using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallData : RoomData
{   /// <summary>
    /// 각 스테이지를 클리어하면 가져온 유물이 전시되어 나타날 유물 변수들(Artifact)
    /// 각 스테이지 씬별 이동을 담당할 Door들
    /// 각 NPC별 대화를 담당할 NPC들이 변수로 설정되어있다.
    /// </summary>
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

    /// <summary>
    /// 플레이어 데이터를 통해 클리어한 스테이지에 대한 정보를 바탕으로 홀이 업데이트된다.
    /// 이미 클리어한 데이터가 존재하면 아티팩트들을 ON
    /// Door들은 이동할 스테이지들을 설정해준다.
    /// 만약 이미 클리어한 씬은 다시 돌아볼 필요가 없기에 다시 들어가지 못하도록 state 업데이트.
    /// </summary>
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

        // 처음 홀에 입장하게 되는 경우와 아닌 경우에 따라 문의 입장 상태가 달라진다!
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
    /// <summary>
    /// 처음 홀에 들어오게된다면 먼저 NPC와 대화를 통해 들어갈 수 있도록
    /// 여러 타입중 need_talk으로 못들어가게 방지
    /// </summary>
    void NeedTalkForOpenDoor()
    {
        jumpMap.SetDoorType(DoorType.need_talk);
        maze.SetDoorType(DoorType.need_talk);
        trap.SetDoorType(DoorType.need_talk);
        treasure.SetDoorType(DoorType.need_talk);
    }
    /// <summary>
    /// 플레이어가 메인 NPC(Cat)과 첫 대화를 나누었다면
    /// 클리어 상태에 맞게 세팅해준다!
    /// </summary>
    void SettingDoor()
    {
        GameManager.Instance.eventStart -= SettingDoor;
        jumpMap.SetDoorType(GameManager.data.clearJumpMap ? DoorType.clear : DoorType.door);
        maze.SetDoorType(GameManager.data.clearMaze ? DoorType.clear : DoorType.door);
        trap.SetDoorType(GameManager.data.clearTrap ? DoorType.clear : DoorType.door);
        treasure.SetDoorType(GameManager.data.clearTreasure ? DoorType.clear : DoorType.door);
    }
}
