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
}
