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

    public void OutScene(PlayableDirector pd)
    {
        GameManager.Instance.curScene.fadeOutAfter += OutCamera;
        if (GameManager.data.tutorial == true) GameManager.Instance.SceneChange(SceneName.Hall);
        else GameManager.Instance.SceneChange(SceneName.StartMap);
    }

    void OutCamera()
    {
        GameManager.Instance.curScene.fadeOutAfter -= OutCamera;
        cameraInfo.depth = 0f;
        deadCamera.Stop();
    }
}