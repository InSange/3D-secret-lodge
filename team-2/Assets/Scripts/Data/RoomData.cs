using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    [SerializeField] Door door;
    [SerializeField] Artifact artifact;
    [SerializeField] List<Transform> monster_spawn;
    [SerializeField] List<Monster> monsters;
    [SerializeField] Scene sceneData;

    public Door GetDoorObject()
    {
        return door;
    }

    public void SetSceneData(Scene data)
    {
        sceneData = data;
        Debug.Log("신데이터 로드:" + sceneData);
    }

    public void RoomSetting()
    {
        artifact.playerGetArtifact += SecondPhase;
        door.SetDoorNextScene(SceneName.Hall);
        door.SetDoorType(DoorType.broken_door);
    }

    public void SecondPhase()
    {
        door.SetDoorType(DoorType.door);
        
        for(int i = 0; i < monsters.Count; i++)
        {
            monsters[i].detectCollider.radius = 300.0f;
        }
    }
}
