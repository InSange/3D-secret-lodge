using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class JumpMapData : RoomData
{/// <summary>
/// 시네머신 기능을 포함한 입장 이벤트 카메라와 두번째 페이즈에 돌입시 발생하는 카메라를 담을 변수 선언.
/// 두번째 페이즈 돌입시 작동될 용암지대
/// </summary>
    [SerializeField] GameObject enterCamera;
    [SerializeField] GameObject secondPhaseCamera;
    [SerializeField] Lava lava;
    /// <summary>
    /// 점프맵 초기 세팅이다.
    /// 유물을 획득하였을때 두번째 페이즈로 변환해줄 이벤트를 추가해주고.
    /// 씬 로드가 끝난후 페이드 아웃이 끝났을 때 초기 입장 카메라 이벤트를 달아주었다.
    /// </summary>
    public override void RoomSetting()
    {
        base.RoomSetting();
        artifact.playerGetArtifact += JumpMapSecondPhase;
        GameManager.Instance.fadeOutAfter += OnEnterCamera;
        secondPhaseCamera.SetActive(false);
    }
    /// <summary>
    /// 유물 획득 후 두번째 페이즈에서 홀로 나갈 수 있도록 문을 설정해두고
    /// 용암이 점점 차오르게 세팅해준다.
    /// </summary>
    public void JumpMapSecondPhase()
    {   // 델리게이트(이벤트) 종료된 이벤트 삭제
        artifact.playerGetArtifact -= JumpMapSecondPhase;
        // 문 나갈수 있게 세팅
        door.SetDoorType(DoorType.door);
        door.doorEvent += OutHall;
        // 용암 세팅
        lava.LavaOn();
        // 시네마틱 2페이즈 카메라 시작
        OnSecondPhaseCamera();
    }
    /// <summary>
    /// 씬 입장시 페이드 아웃이 끝날때 이벤트이다.
    /// </summary>
     void OnEnterCamera()
    {
        GameManager.Instance.canInput = false;
        GameManager.Instance.fadeOutAfter -= OnEnterCamera;
        PlayableDirector pd = enterCamera.GetComponent<PlayableDirector>();
        pd.stopped += OffEnterCamera;
        enterCamera.SetActive(true);
    }
    /// <summary>
    /// 초기 이벤트 카메라가 종료될때 해당 카메라 사용을 막는다.
    /// </summary>
    /// <param name="pd"></param>
    void OffEnterCamera(PlayableDirector pd)
    {
        enterCamera.SetActive(false);
        GameManager.Instance.canInput = true;
    }
    // 이건 시작할때 -> 페이즈 시작은 유물먹었을때
    /// <summary>
    /// 유물을 먹었을 때 실행되는 두번째 이벤트 카메라
    /// 첫번째 이벤트 카메라와 동일하게 카메라 이벤트가 종료후 해당 카메라 사용을 중지한다.
    /// </summary>
    void OnSecondPhaseCamera()
    {   // 시네마틱 영상 시작중일때 플레이어 조작 X
        GameManager.Instance.canInput = false;
        PlayableDirector pd = secondPhaseCamera.GetComponent<PlayableDirector>();
        pd.stopped += OffSecondPhaseCamera;
        secondPhaseCamera.SetActive(true);
    }
    void OffSecondPhaseCamera(PlayableDirector pd)
    {
        secondPhaseCamera.SetActive(false);
        GameManager.Instance.canInput = true;
    }
    /// <summary>
    /// 홀로 나가는 이벤트로 문과 상호작용할때 홀로 씬 전환이 이루어지면서 데이터를 저장해준다.
    /// </summary>
    public override void OutHall()
    {
        base.OutHall();
        GameManager.data.clearJumpMap = true;
        GameManager.SaveGameData();
        door.doorEvent -= OutHall;
        Debug.Log("점프맵 세이브 완료!");
    }
}
