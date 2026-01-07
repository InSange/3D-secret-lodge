using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Hall : Scene
 {  /// <summary>
    /// 초기 변수들
    /// initCamera - 입장시 시작하는 시네머신 카메라
    /// </summary>
    // Camera
    [SerializeField] GameObject initCamera;

    /// <summary>
    /// Load object and data to Hall
    /// </summary>
    public override void load()
    {
        base.Update();
        Init();
    }

    private void Init()
    {
        LoadMap();

        LoadPlayer("hall spawn");

        LoadFinish();
        // Hall에 아예 처음 방문했을시 ( 플레이어 데이터 비교 )
        if (GameManager.data.visitedHall == false)
        {   // 처음 입장하는 이벤트를 실행
            GameManager.Instance.fadeOutAfter += HallInitEnter;
        }
    }

    /// <summary>
    /// 홀을 구성하는 배경과 룸 데이터 세팅.
    /// </summary>
    void LoadMap()
    {
        // Load BackGround Map
        map = Instantiate((GameObject)Resources.Load("Scene/Hall/Hall"));
        map.transform.SetParent(this.transform);

        room = GetComponentInChildren<RoomData>();
        room.SetSceneData(this);
        room.RoomSetting();

        initCamera = GameObject.Find("Init Camera");
        initCamera.SetActive(false);
        PlayableDirector finishDirector = initCamera.GetComponent<PlayableDirector>();
        finishDirector.stopped += OffCamera;

    }
    /// <summary>
    /// 홀에 처음 들어왔을 경우
    /// 준비되어 있던 시네머신(애니메이션)을 진행한다!
    /// </summary>
    void HallInitEnter()
    {
        if (GameManager.Instance.fadeOutAfter != null) GameManager.Instance.fadeOutAfter -= HallInitEnter;
        initCamera.SetActive(true);
    }    
    /// <summary>
    /// 시네머신 (애니메이션) 이 끝나면 종료하고 대화 이벤트를 시작한다!
    /// </summary>
    /// <param name="pd"></param>
    void OffCamera(PlayableDirector pd)
    {   
        initCamera.SetActive(false);
        UIManager.Instance.finishDialogue -= HallInitEnter;
        UIManager.Instance.StartDialogue(EventDialogue.SeeTheCat);
    }
}
