using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureData : RoomData
{
    [SerializeField] GameObject enterCamera;
    [SerializeField] List<Monster> monsters;
    [SerializeField] List<GameObject> boxes;
    [SerializeField] List<GameObject> monsterInBox;
    [SerializeField] bool isArtifactInput;
    // Start is called before the first frame update
    void Start()
    {
        artifact.playerGetArtifact += TreasureSecondPhase;
    }

    public override void RoomSetting()
    {
        base.RoomSetting();
        boxes[0].GetComponent<TreasureBox>().SetHaveArtifact(false, null);

        for(int i = 1; i < boxes.Count; i++)
        {
            if(!isArtifactInput)
            {
                int rand = Random.Range(0, 1);
                if (rand == 1)
                {
                    boxes[i].GetComponent<TreasureBox>().SetHaveArtifact(true, artifact.gameObject);
                    artifact.transform.SetParent(boxes[i].transform);
                    artifact.transform.localPosition = Vector3.zero;
                }
                else
                {
                    boxes[i].GetComponent<TreasureBox>().SetHaveArtifact(false, monsterInBox[0]);
                    monsterInBox[0].transform.SetParent(boxes[i].transform);
                    monsterInBox[0].transform.localPosition = Vector3.zero;
                    monsterInBox.RemoveAt(0);
                }
            }
            else
            {
                boxes[i].GetComponent<TreasureBox>().SetHaveArtifact(false, monsterInBox[0]);
                monsterInBox[0].transform.SetParent(boxes[i].transform);
                monsterInBox[0].transform.localPosition = Vector3.zero;
                monsterInBox.RemoveAt(0);
            }
        }
    }

    public void InitTreasureRoom()
    {

    }

    public void TreasureSecondPhase()
    {
        door.SetDoorType(DoorType.door);

        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].detectCollider.radius = 300.0f;
        }
    }
}
