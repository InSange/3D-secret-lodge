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
        Debug.Log("�ŵ����� �ε�:" + sceneData);
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
        Debug.Log("�� Ż��!");
    }

    void GetOutArtifact()
    {
        GameManager.Instance.fadeOutAfter -= GetOutArtifact;
        int GetDialogueNum = 100 + GameManager.data.getArtifactNum;
        UIManager.Instance.StartDialogue((EventDialogue)GetDialogueNum);
    }
}

///
/// ���� ȹ�� �� -> OutHall 
/// OutHall ����� ���̺� ���� ����� �Բ� RoomScene -> HallScene ���� ��ȯ
///  OutHall ������ FadeIn �̺�Ʈ ȣ��
///  ���ο� ������ FadeOut �̺�Ʈ ȣ��?
///