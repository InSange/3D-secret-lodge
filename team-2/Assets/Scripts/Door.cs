using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]SceneName nextScene;

    public void SetDoorNextScene(SceneName scene)
    {
        nextScene = scene;
    }

    public SceneName GetNextScene()
    {
        return nextScene;
    }
}
