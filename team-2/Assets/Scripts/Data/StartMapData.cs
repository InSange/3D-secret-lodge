using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMapData : RoomData
{
    public override void RoomSetting()
    {
        Debug.Log("��ŸƮ�� ����");
        door.doorEvent += InHall;
    }

    void InHall()
    {
        GameManager.data.tutorial = true;
        GameManager.SaveGameData();
        door.doorEvent -= InHall;
        Debug.Log("��ŸƮ�� ���̺� �Ϸ�!");
    }
}
