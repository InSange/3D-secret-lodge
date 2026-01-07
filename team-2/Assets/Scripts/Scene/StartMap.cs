using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 인트로에서 처음 플레이를 시작하는 씬이다.
/// </summary>
public class StartMap : Scene
{
    protected override void Update()
    {
        base.Update();
    }
    /// <summary>
    /// Load object and data to StartMap
    /// </summary>
    public override void load()
    {
        base.load();

        LoadMapData();

        LoadPlayer();

        LoadFinish();
        // 페이드 아웃이 끝나면 처음 시작하는 튜토리얼 메시지를 실행한다!
        GameManager.Instance.fadeOutAfter += TutorialMessage;
    }
    /// <summary>
    /// 시작맵을 구성하는 배경 오브젝트와 자식 오브젝트로 들어가있는
    /// 룸 데이터를 찾아서 불러온다.
    /// 룸 데이터와 시작 맵 씬을 연결시키고 초기 세팅을 실행시켜준다.
    /// </summary>
    void LoadMapData()
    {   // Load BackGround Map
        map = Instantiate((GameObject)Resources.Load("Scene/StartMap/Start_MAP"));
        map.transform.SetParent(this.transform);
        room = GetComponentInChildren<RoomData>();
        room.SetSceneData(this);
        room.RoomSetting();
    }
    /// <summary>
    /// 게임이 시작되면 튜토리얼 메시지를 이벤트함수에서 제거해주고
    /// 튜토리얼 메시지를 실행시켜주면서 본격적으로 게임이 시작된다.
    /// </summary>
    void TutorialMessage()
    {
        GameManager.Instance.fadeOutAfter -= TutorialMessage;
        Cursor.visible = false;
        GameManager.Instance.isPlaying = true;
        UIManager.Instance.StartDialogue(EventDialogue.StartMountain);
        // TutorialMessage();
    }
}
