using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Hall : Scene
{
    [SerializeField] GameObject map;

    // Door;
    [SerializeField] GameObject entrance;
    [SerializeField] GameObject jumpMap;
    [SerializeField] GameObject maze;
    [SerializeField] GameObject quiz;
    [SerializeField] GameObject treasure;

    // NPC Cat
    [SerializeField] NPC cat;

    // Camera
    [SerializeField] GameObject initCamera;

    // Scene State
    [SerializeField] bool isDescription;
    [SerializeField] int sequence;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void Init()
    {
        LoadMap();

        LoadPlayer("hall spawn");

        LoadFinish();
        HallInitEnter();
    }

    void LoadMap()
    {
        // Load BackGround Map
        map = Instantiate((GameObject)Resources.Load("Scene/Hall/Hall"));
        map.transform.SetParent(this.transform);

        entrance = GameObject.Find("Entrance Door");
        jumpMap = GameObject.Find("JumpMap Door");
        maze = GameObject.Find("Maze Door");
        quiz = GameObject.Find("Quiz Door");
        treasure = GameObject.Find("Treasure Door");

        cat = GameObject.Find("NPC_CAT").GetComponent<NPC>();

        Door sceneDoor;
        sceneDoor = entrance.AddComponent<Door>();
        sceneDoor.SetDoorNextScene(SceneName.CantMoveScene);
        sceneDoor.SetDoorType(DoorType.broken_door);

        sceneDoor = jumpMap.AddComponent<Door>();
        sceneDoor.SetDoorNextScene(SceneName.JumpMap);
        sceneDoor.SetDoorType(DoorType.door);

        sceneDoor = maze.AddComponent<Door>();
        sceneDoor.SetDoorNextScene(SceneName.Maze);
        sceneDoor.SetDoorType(DoorType.door);

        sceneDoor = quiz.AddComponent<Door>();
        sceneDoor.SetDoorNextScene(SceneName.Quiz);
        sceneDoor.SetDoorType(DoorType.door);

        sceneDoor = treasure.AddComponent<Door>();
        sceneDoor.SetDoorNextScene(SceneName.Treasure);
        sceneDoor.SetDoorType(DoorType.door);

        initCamera = GameObject.Find("Init Camera");
        initCamera.SetActive(false);
        PlayableDirector finishDirector = initCamera.GetComponent<PlayableDirector>();
        finishDirector.stopped += OffCamera;
        sequence = 0;

        UIManager.Instance.finishDialogue += HallInitEnter;
    }

    void HallInitEnter()
    {
        Debug.Log(sequence + "응애 시작!");
        switch (sequence)
        {
            case 0:
                UIManager.Instance.StartDialogue(EventDialogue.InHall);
                break;
            case 1:
                initCamera.SetActive(true);
                break;
            default:
                break;
        }
        sequence++;
    }    

    void OffCamera(PlayableDirector pd)
    {
        initCamera.SetActive(false);
        UIManager.Instance.StartDialogue(EventDialogue.TalkWithCat);
    }
}
