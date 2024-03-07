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
    {   // ��������Ʈ(�̺�Ʈ) ����� �̺�Ʈ ����
        artifact.playerGetArtifact -= JumpMapSecondPhase;
        // �� ������ �ְ� ����
        door.SetDoorType(DoorType.door);
        door.doorEvent += OutHall;
        // ��� ����
        lava.LavaOn();
        // �ó׸�ƽ 2������ ī�޶� ����
        OnSecondPhaseCamera();
    }
    // �� ����� ���̵� �ƿ� ������
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
    // �̰� �����Ҷ� -> ������ ������ �����Ծ�����
    void OnSecondPhaseCamera()
    {   // �ó׸�ƽ ���� �������϶� �÷��̾� ���� X
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
        Debug.Log("������ ���̺� �Ϸ�!");
    }
}
