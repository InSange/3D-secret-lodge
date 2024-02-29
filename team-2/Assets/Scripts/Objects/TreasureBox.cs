using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    public GameObject reward;
    public GameObject box;

    public void OpenBox()
    {
        this.gameObject.SetActive(false);
        if(boxContent != null) boxContent.SetActive(true);
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
