using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    broken_door = 0,
    door
}

public class Door : MonoBehaviour
{
    [SerializeField] SceneName nextScene;
    [SerializeField] DoorType type;

    public void SetDoorNextScene(SceneName scene)
    {
        nextScene = scene;
    }

    public SceneName GetNextScene()
    {
        return nextScene;
    }

    public void SetDoorType(DoorType t)
    {
        type = t;
    }

    public DoorType GetDoorType()
    {
        return type;
    }
}
