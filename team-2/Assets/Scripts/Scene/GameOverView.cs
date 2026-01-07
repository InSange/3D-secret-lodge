using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameOverView : MonoBehaviour
{/// <summary>
/// 게임오버 이벤트를 위한 변수들
/// </summary>
    [SerializeField] Camera cameraInfo; // 카메라의 깊이 값을 높여줌으로써 플레이어 화면 -> 게임 오버 화면 전환 시키기 위한 변수
    [SerializeField] PlayableDirector deadCamera;   // PlayableDirector로 원하는 시점에 이벤트를 발생할 수 있도록 하기 위한 변수
    // 게임오버 연출을 위한 오브젝트들
    [SerializeField] GameObject zombie; 
    [SerializeField] GameObject mutant;
    [SerializeField] GameObject crusader;
    [SerializeField] GameObject titan;
    [SerializeField] GameObject rocks;
    [SerializeField] GameObject axe;
    [SerializeField] GameObject lava;
    // 영상이 끝날때 마무리를 위한 델리게이트 함수
    public delegate void DeadEvent();
    public DeadEvent playerDeadEvent;
    /// <summary>
    /// 각 몬스터 타입에 맞게 오브젝트 활성화 및 종료 이벤트 추가.
    /// </summary>
    /// <param name="monster"></param>
    public void eventStart(MonsterType monster)
    {
        switch (monster)
        {
            case MonsterType.Zombie:
                zombie.SetActive(true);
                playerDeadEvent += ZombieEvent;
                break;
            case MonsterType.Mutant:
                mutant.SetActive(true);
                playerDeadEvent += MutantEvent;
                break;
            case MonsterType.Crusader:
                crusader.SetActive(true);
                playerDeadEvent += CrusaderEvent;
                break;
            case MonsterType.Titan:
                titan.SetActive(true);
                playerDeadEvent += TitanEvent;
                break;
            case MonsterType.Rocks:
                rocks.SetActive(true);
                playerDeadEvent += RockEvent;
                break;
            case MonsterType.Axe:
                axe.SetActive(true);
                playerDeadEvent += AxeEvent;
                break;
            case MonsterType.Lava:
                lava.SetActive(true);
                playerDeadEvent += LavaEvent;
                break;
            default:
                break;
        }
        // 카메라의 depth를 높여줌으로써 카메라 전환이 이루어진다.
        // stopped는 playable director의 time line이 종료될때 이벤트로 마무리 컷신을 장식해주기 위해 달아주었다.
        cameraInfo.depth = 5.0f;
        deadCamera.stopped += OutScene;
        deadCamera.Play();  // playable director 실행!
    }

    void ZombieEvent()
    {
        playerDeadEvent -= ZombieEvent;
        zombie.SetActive(false);
    }

    void MutantEvent()
    {
        playerDeadEvent -= MutantEvent;
        mutant.SetActive(false);
    }

    void CrusaderEvent()
    {
        playerDeadEvent -= CrusaderEvent;
        crusader.SetActive(false);
    }

    void TitanEvent()
    {
        playerDeadEvent -= TitanEvent;
        titan.SetActive(false);
    }

    void RockEvent()
    {
        playerDeadEvent -= RockEvent;
        rocks.SetActive(false);
    }

    void AxeEvent()
    {
        playerDeadEvent -= AxeEvent;
        axe.SetActive(false);
    }

    void LavaEvent()
    {
        playerDeadEvent -= LavaEvent;
        lava.SetActive(false);
    }
    /// <summary>
    /// 게임오버가 실행되면 다시 시작할 위치로 튜토리얼 클리어 유무에 따라
    /// 시작 맵 혹은 홀로 귀환된다.
    /// </summary>
    /// <param name="pd"></param>
    public void OutScene(PlayableDirector pd)
    {
        if (GameManager.data.tutorial == true) GameManager.Instance.SceneChange(SceneName.Hall);
        else GameManager.Instance.SceneChange(SceneName.StartMap);
        GameManager.Instance.fadeInFinish += OutCamera;
    }
    /// <summary>
    /// 게임 오버 카메라 -> 플레이어 카메라로 다시 전환시켜준다.
    /// </summary>
    void OutCamera()
    {
        GameManager.Instance.fadeInFinish -= OutCamera;
        if(playerDeadEvent != null) playerDeadEvent();
        cameraInfo.depth = 0f;
        deadCamera.Stop();
    }
}