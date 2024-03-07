using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class JumpMapData : RoomData
{
    [SerializeField] GameObject enterCamera;
    [SerializeField] GameObject secondPhaseCamera;
    [SerializeField] Lava lava;

    // Start is called before the first frame update
    void Start()
    {
        artifact.playerGetArtifact += JumpMapSecondPhase;
    }

    public override void RoomSetting()
    {
        base.RoomSetting();
        GameManager.Instance.fadeOutAfter += OnEnterCamera;
        secondPhaseCamera.SetActive(false);
    }

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
    // 씬 입장시 페이드 아웃 끝날때
     void OnEnterCamera()
    {
        GameManager.Instance.canInput = false;
        GameManager.Instance.fadeOutAfter -= OnEnterCamera;
        PlayableDirector pd = enterCamera.GetComponent<PlayableDirector>();
        pd.stopped += OffEnterCamera;
        enterCamera.SetActive(true);
    }

    void OffEnterCamera(PlayableDirector pd)
    {
        enterCamera.SetActive(false);
        GameManager.Instance.canInput = true;
    }
    // 이건 시작할때 -> 페이즈 시작은 유물먹었을때
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

    public override void OutHall()
    {
        base.OutHall();
        GameManager.data.clearJumpMap = true;
        GameManager.SaveGameData();
        door.doorEvent -= OutHall;
        Debug.Log("점프맵 세이브 완료!");
    }
}
