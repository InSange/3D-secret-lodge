using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 각 씬들이 가지고 있는 데이터(오브젝트)
/// 들을 따로 RoomData로 빼두었다.
/// 해당 스크립트 역시 상속으로 공통적인 부분을 빼두었다.
/// </summary>
public class RoomData : MonoBehaviour
{   /// <summary>
    /// 각 씬들은 씬 전환을 위한 문이 존재하고
    /// 퀘스트 클리어를 위한 아티팩트와 
    /// 씬과 데이터끼리 서로 드나들 수 있도록 Scene 변수를 만들어주었다.
    /// </summary>
    public Door door;
    public Artifact artifact;
    public Scene sceneData;
    /// <summary>
    /// 해당 룸 데이터를 사용하는 씬에 접근할 수 있도록 함수 선언.
    /// </summary>
    /// <param name="data"></param>
    public void SetSceneData(Scene data)
    {
        sceneData = data;
    }
    /// <summary>
    /// 상속하는 모든 하위 클래스들은 따로 선언해주지 않는다면
    /// 기본적으로 문은 홀과 이어져있으며 이 문은 사용할 수 없다.
    /// </summary>
    public virtual void RoomSetting()
    {
        door.SetDoorNextScene(SceneName.Hall);
        door.SetDoorType(DoorType.broken_door);
    }
    /// <summary>
    /// 미션(Artifact)를 클리어하고 홀로 빠져나가는 문과 상호작용했을 때
    /// 클리어 정보를 저장하기 위해서 이벤트를 추가해주는 함수.
    /// </summary>
    public virtual void OutHall()
    {
        GameManager.data.getArtifactNum++;
        GameManager.Instance.fadeOutAfter += GetOutArtifact;
    }
    /// <summary>
    /// 미션 클리어한 데이터를 저장하고 저장한 데이터를 바탕으로 대화 이벤트 실행.
    /// </summary>
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