using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMapData : RoomData
{
    public override void RoomSetting()
    {
        Debug.Log("스타트룸 세팅");
        door.doorEvent += InHall;
    }

    void InHall()
    {
        GameManager.data.tutorial = true;
        GameManager.SaveGameData();
        door.doorEvent -= InHall;
        Debug.Log("스타트맵 세이브 완료!");
    }
}
