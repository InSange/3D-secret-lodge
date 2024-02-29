using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TrapRoomData : RoomData
{
    [SerializeField] GameObject enterCamera;
    [SerializeField] GameObject secondPhaseCamera;

    // Start is called before the first frame update
    void Start()
    {
        artifact.playerGetArtifact += TrapRoomSecondPhase;
    }

    public override void RoomSetting()
    {
        base.RoomSetting();
        sceneData.fadeOutAfter += OnEnterCamera;
        secondPhaseCamera.SetActive(false);
    }

    private void OnEnterCamera()
    {
        GameManager.Instance.canInput = false;
        sceneData.fadeOutAfter -= OnEnterCamera;
        PlayableDirector pd = enterCamera.GetComponent<PlayableDirector>();
        pd.stopped += OffEnterCamera;
        enterCamera.SetActive(true);
    }

    void OffEnterCamera(PlayableDirector pd)
    {
        enterCamera.SetActive(false);
        GameManager.Instance.canInput = true;
    }

    private void TrapRoomSecondPhase()
    {
        artifact.playerGetArtifact -= TrapRoomSecondPhase;
        door.SetDoorType(DoorType.door);
        OnSecondPhaseCamera();
    }

    private void OnSecondPhaseCamera()
    {
        GameManager.Instance.canInput = false;
        PlayableDirector pd = secondPhaseCamera.GetComponent<PlayableDirector>();
        pd.stopped += OffSecondPhaseCamera;
        secondPhaseCamera.SetActive(true);
    }

    private void OffSecondPhaseCamera(PlayableDirector obj)
    {
        secondPhaseCamera.SetActive(false);
        GameManager.Instance.canInput = true;
    }
}
