using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TrapRoomData : RoomData
{/// <summary>
/// 점프맵과 동일하게 입장시 발생하는 이벤트 카메라와 유물 획득 후 2페이즈 이벤트 카메라를 관리한다.
/// 유물을 가지러 가는 길목에 있는 함정 오브젝트들에 대한 데이터를 담고있다.
/// </summary>
    [SerializeField] GameObject enterCamera;
    [SerializeField] GameObject secondPhaseCamera;
    [SerializeField] List<TrapOBJ> trapOBJ;
    /// <summary>
    /// 함정맵 초기 세팅이다.
    /// 입장시 발생하는 이벤트 카메라와
    /// 유물 획득 시 두번째 페이즈로 전환해줄 이벤트 함수를 추가해주었다.
    /// </summary>
    public override void RoomSetting()
    {
        base.RoomSetting();
        artifact.playerGetArtifact += TrapRoomSecondPhase;
        GameManager.Instance.fadeOutAfter += OnEnterCamera;
        secondPhaseCamera.SetActive(false);
    }
    /// <summary>
    /// 입장시 발생하는 이벤트 카메라
    /// </summary>
    private void OnEnterCamera()
    {
        GameManager.Instance.canInput = false;
        GameManager.Instance.fadeOutAfter -= OnEnterCamera;
        PlayableDirector pd = enterCamera.GetComponent<PlayableDirector>();
        pd.stopped += OffEnterCamera;
        enterCamera.SetActive(true);
    }
    /// <summary>
    /// 입장 카메라 종료 세팅
    /// </summary>
    /// <param name="pd"></param>
    void OffEnterCamera(PlayableDirector pd)
    {
        enterCamera.SetActive(false);
        GameManager.Instance.canInput = true;
    }
    /// <summary>
    /// 두번째 페이즈에 돌입하면 들어왔던 문은 다시 홀로 나갈 수 있도록 세팅해주고
    /// 각 함정들은 두번째 페이즈에 돌입하게 된다.
    /// </summary>
    private void TrapRoomSecondPhase()
    {
        artifact.playerGetArtifact -= TrapRoomSecondPhase;
        door.SetDoorType(DoorType.door);
        door.doorEvent += OutHall;
        for (int i = 0; i < trapOBJ.Count; i++)
        {
            trapOBJ[i].SecondPhaseSetting();
        }
        OnSecondPhaseCamera();
    }
    /// <summary>
    /// 두번째 페이즈 카메라 시작이다!
    /// </summary>
    private void OnSecondPhaseCamera()
    {
        GameManager.Instance.canInput = false;
        PlayableDirector pd = secondPhaseCamera.GetComponent<PlayableDirector>();
        pd.stopped += OffSecondPhaseCamera;
        secondPhaseCamera.SetActive(true);
    }
    /// <summary>
    /// 두번째 페이즈 카메라 종료 세팅
    /// </summary>
    /// <param name="obj"></param>
    private void OffSecondPhaseCamera(PlayableDirector obj)
    {
        secondPhaseCamera.SetActive(false);
        GameManager.Instance.canInput = true;
    }
    /// <summary>
    /// 홀로 나갈시 데이터 저장 이벤트 달아주기!
    /// </summary>
    public override void OutHall()
    {
        base.OutHall();
        GameManager.data.clearTrap= true;
        GameManager.SaveGameData();
        door.doorEvent -= OutHall;
        Debug.Log("함정맵 세이브 완료!");
    }
}
