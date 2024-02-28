using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    [SerializeField] bool haveArtifact;
    [SerializeField] GameObject boxContent;

    public void OpenBox()
    {
        this.gameObject.SetActive(false);
    }

    public void SetHaveArtifact(bool flag, GameObject obj)
    {
        haveArtifact = flag;
        boxContent = obj;
    }

    public bool GetHaveArtifact()
    {
        return haveArtifact;
    }
}
