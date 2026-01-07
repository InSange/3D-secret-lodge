using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMapData : RoomData
{/// <summary>
/// 미로에는 여러 종류의 몬스터들이 존재한다!
/// </summary>
    [SerializeField] List<Monster> monsters;
    /// <summary>
    /// 미로에는 별도의 이벤트 카메라가 존재하지 않고
    /// 플레이어가 유물을 획득했을때 몬스터가 플레이어를 무조건 쫓아오는 2페이즈가 준비되어있다.
    /// </summary>
    public override void RoomSetting()
    {
        base.RoomSetting();
        artifact.playerGetArtifact += MazeSecondPhase;
    }
    /// <summary>
    /// 입장했던 문은 홀로 나갈 수 있도록 세팅해주고
    /// 미로에 존재하는 몬스터들의 탐지 범위를 크게해서 무조건 플레이어가 탐지되게끔 설정해주었다.
    /// </summary>
    public void MazeSecondPhase()
    {
        door.SetDoorType(DoorType.door);
        door.doorEvent += OutHall;

        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].detectCollider.radius = 300.0f;
        }
    }
    /// <summary>
    /// 문을 통해 홀을 나가게되면 데이터를 저장해준다.
    /// </summary>
    public override void OutHall()
    {
        base.OutHall();
        GameManager.data.clearMaze= true;
        GameManager.SaveGameData();
        door.doorEvent -= OutHall;
        Debug.Log("미로 세이브 완료!");
    }
}
