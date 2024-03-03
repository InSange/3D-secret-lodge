using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallData : RoomData
{
    [SerializeField] GameObject jumpMapArtifact;
    [SerializeField] GameObject mazeArtifact;
    [SerializeField] GameObject treasureArtifact;
    [SerializeField] GameObject trapArtifact;

    public override void RoomSetting()
    {
        if (GameManager.data.clearJumpMap) jumpMapArtifact.SetActive(true);
        if (GameManager.data.clearMaze) mazeArtifact.SetActive(true);
        if (GameManager.data.clearTreasure) treasureArtifact.SetActive(true);
        if (GameManager.data.clearTrap) trapArtifact.SetActive(true);
    }
}
