using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameOverView : MonoBehaviour
{
    [SerializeField] Camera cameraInfo;
    [SerializeField] PlayableDirector deadCamera;
    [SerializeField] GameObject zombie;
    [SerializeField] GameObject mutant;
    [SerializeField] GameObject crusader;
    [SerializeField] GameObject titan;
    [SerializeField] GameObject rocks;
    [SerializeField] GameObject axe;
    [SerializeField] GameObject lava;

    public delegate void DeadEvent();
    public DeadEvent playerDeadEvent;

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

        cameraInfo.depth = 5.0f;
        deadCamera.stopped += OutScene;
        deadCamera.Play();
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

    public void OutScene(PlayableDirector pd)
    {
        Debug.Log("왜 안됑?12313213");
        if (GameManager.data.tutorial == true) GameManager.Instance.SceneChange(SceneName.Hall);
        else GameManager.Instance.SceneChange(SceneName.StartMap);
        GameManager.Instance.fadeInFinish += OutCamera;
    }

    void OutCamera()
    {
        Debug.Log("왜 안됑?44");
        GameManager.Instance.fadeInFinish -= OutCamera;
        playerDeadEvent();
        cameraInfo.depth = 0f;
        deadCamera.Stop();
    }
}