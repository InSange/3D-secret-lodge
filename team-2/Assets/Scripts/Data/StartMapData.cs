using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMapData : RoomData
{   /// <summary>
    /// 오브젝트들의 초기 세팅으로 시작맵에 존재하는 문에 문 이벤트로
    /// 다음씬인 홀로들어갈때 발생시켜줄 이벤트를 넣어준다.
    /// </summary>
    public override void RoomSetting()
    {
        door.doorEvent += InHall;
    }
    /// <summary>
    /// 홀로 들어가는 문과 상호작용을 실행했다면
    /// 시작맵의 역할인 튜토리얼은 끝이므로 플레이어 데이터를 저장해준다.
    /// </summary>
    void InHall()
    {   
        GameManager.data.tutorial = true;
        GameManager.SaveGameData();
        door.doorEvent -= InHall;
    }
}
