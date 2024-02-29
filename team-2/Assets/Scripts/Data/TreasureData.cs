using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TreasureData : RoomData
{
    [SerializeField] GameObject enterCamera;
    [SerializeField] GameObject monsterParent;
    [SerializeField] List<GameObject> monsters;
    [SerializeField] List<GameObject> boxes;
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

        int rand = Random.Range(1, boxes.Count);

        for(int i = 1; i < boxes.Count; i++)
        {
            if (i == rand)
            {
                boxes[i].GetComponent<TreasureBox>().SetHaveArtifact(true, artifact.gameObject);
                artifact.transform.localPosition = boxes[i].transform.localPosition;
                artifact.gameObject.SetActive(false);
            }
            else
            {
                GameObject monster = Instantiate((GameObject)Resources.Load("Monster/Hide Zombie"));
                boxes[i].GetComponent<TreasureBox>().SetHaveArtifact(false, monster);
                monster.transform.SetParent(monsterParent.transform);
                monster.transform.localPosition = boxes[i].transform.localPosition;
                monsters.Add(monster);
                monster.SetActive(false);
            }
        }

        sceneData.fadeOutAfter += InitTreasureRoom;
    }

    public void InitTreasureRoom()
    {
        GameManager.Instance.canInput = false;
        sceneData.fadeOutAfter -= InitTreasureRoom;
        PlayableDirector pd = enterCamera.GetComponent<PlayableDirector>();
        pd.stopped += OffEnterCamera;
        enterCamera.SetActive(true);
    }


    public void TreasureSecondPhase()
    {
        door.SetDoorType(DoorType.door);

        for (int i = 0; i < monsters.Count; i++)
        {
            Monster monsterCS = monsters[i].GetComponent<Monster>();
            monsterCS.detectCollider.radius = 300.0f;
        }
    }
    void OffEnterCamera(PlayableDirector pd)
    {
        enterCamera.SetActive(false);
        GameManager.Instance.canInput = true;
    }
}
