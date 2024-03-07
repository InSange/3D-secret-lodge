using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public Door door;
    public Artifact artifact;
    public Scene sceneData;

    public void SetSceneData(Scene data)
    {
        sceneData = data;
        Debug.Log("신데이터 로드:" + sceneData);
    }

    public virtual void RoomSetting()
    {
        door.SetDoorNextScene(SceneName.Hall);
        door.SetDoorType(DoorType.broken_door);
    }

    public virtual void OutHall()
    {
        GameManager.data.getArtifactNum++;
        GameManager.Instance.fadeOutAfter += GetOutArtifact;
        Debug.Log("방 탈출!");
    }

    void GetOutArtifact()
    {
        GameManager.Instance.fadeOutAfter -= GetOutArtifact;
        int GetDialogueNum = 100 + GameManager.data.getArtifactNum;
        UIManager.Instance.StartDialogue((EventDialogue)GetDialogueNum);
    }
}

///
/// 유물 획득 후 -> OutHall 
/// OutHall 실행시 세이브 파일 저장과 함께 RoomScene -> HallScene 으로 전환
///  OutHall 에서는 FadeIn 이벤트 호출
///  새로운 씬에선 FadeOut 이벤트 호출?
///