using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TreasureData : RoomData
{/// <summary>
/// 보물찾기 맨 처음 입장시 이벤트 카메라와 몬스터들을 관리하는 변수들이다.
/// </summary>
    [SerializeField] GameObject enterCamera;
    [SerializeField] GameObject monsterParent;
    [SerializeField] List<GameObject> monsters;
    [SerializeField] List<GameObject> boxes;
    /// <summary>
    /// 보물찾기맵 초기 세팅이다.
    /// 유물 획득 시 두번째 페이즈 시작하는 이벤트를 추가해준다.
    /// 보물찾기 맵에는 보물 상자들이 여러개 있고 그 중 하나에다가 유물을 넣어준다.
    /// 유물 외에는 몬스터들이 들어있다!
    /// </summary>
    public override void RoomSetting()
    {
        base.RoomSetting();
        artifact.playerGetArtifact += TreasureSecondPhase;
        boxes[0].GetComponent<TreasureBox>().SetHaveArtifact(false, null);
        // 유물을 넣을 상자 번호 랜덤으로 뽑기
        int rand = Random.Range(1, boxes.Count);

        for(int i = 1; i < boxes.Count; i++)
        {
            if (i == rand)
            {   // i가 랜덤 번호라면 유물 게임오브젝트를 넣어준다.
                boxes[i].GetComponent<TreasureBox>().SetHaveArtifact(true, artifact.gameObject);
                artifact.transform.localPosition = boxes[i].transform.localPosition;
                artifact.gameObject.SetActive(false);
            }
            else
            {   // i가 랜덤 번호가 아니라면 좀비를 넣어준다.
                GameObject monster = Instantiate((GameObject)Resources.Load("Monster/Hide Zombie"));
                boxes[i].GetComponent<TreasureBox>().SetHaveArtifact(false, monster);
                monster.transform.SetParent(monsterParent.transform);
                monster.transform.localPosition = boxes[i].transform.localPosition;
                monsters.Add(monster);
                monster.SetActive(false);
            }
        }
        // 페이드 아웃 후 초기 이벤트 카메라 설정
        GameManager.Instance.fadeOutAfter += InitTreasureRoom;
    }
    /// <summary>
    /// 카메라 이벤트가 끝나면 카메라 해제해준다.
    /// </summary>
    public void InitTreasureRoom()
    {   
        GameManager.Instance.canInput = false;
        GameManager.Instance.fadeOutAfter -= InitTreasureRoom;
        PlayableDirector pd = enterCamera.GetComponent<PlayableDirector>();
        pd.stopped += OffEnterCamera;
        enterCamera.SetActive(true);
    }
    /// <summary>
    /// 보물찾기맵 두번째 페이즈 시작
    /// 맵에 존재하는 몬스터들은 탐지 거리가 대폭 상승하고
    /// 무조건 플레이어를 쫓아온다.
    /// 그리고 문은 홀로 나갈 수 있도록 세팅된다.
    /// </summary>
    public void TreasureSecondPhase()
    {
        door.SetDoorType(DoorType.door);
        door.doorEvent += OutHall;

        for (int i = 0; i < monsters.Count; i++)
        {
            Monster monsterCS = monsters[i].GetComponent<Monster>();
            monsterCS.detectCollider.radius = 300.0f;
        }
    }
    /// <summary>
    /// 이벤트 카메라 종료!
    /// </summary>
    /// <param name="pd"></param>
    void OffEnterCamera(PlayableDirector pd)
    {
        enterCamera.SetActive(false);
        GameManager.Instance.canInput = true;
    }
    /// <summary>
    /// 홀로 나가는 이벤트로 문과 상호작용시 홀로 씬 전환이 이뤄지면서 데이터를 저장.
    /// </summary>
    public override void OutHall()
    {
        base.OutHall();
        GameManager.data.clearTreasure = true;
        GameManager.SaveGameData();
        door.doorEvent -= OutHall;
        Debug.Log("함정맵 세이브 완료!");
    }
}
