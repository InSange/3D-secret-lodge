using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Hall : Scene
{
    [SerializeField] GameObject map;

    // Door;
    [SerializeField] GameObject jumpMap;
    [SerializeField] GameObject maze;
    [SerializeField] GameObject trap;
    [SerializeField] GameObject treasure;

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

        if (GameManager.data.visitedHall == false)
        {
            fadeOutAfter += HallInitEnter;
        }
    }

    void LoadMap()
    {
        // Load BackGround Map
        map = Instantiate((GameObject)Resources.Load("Scene/Hall/Hall"));
        map.transform.SetParent(this.transform);

        room = GetComponentInChildren<RoomData>();
        room.SetSceneData(this);
        room.RoomSetting();

        jumpMap = GameObject.Find("JumpMap Door");
        maze = GameObject.Find("Maze Door");
        trap = GameObject.Find("Trap Door");
        treasure = GameObject.Find("Treasure Door");

        Door sceneDoor;

        sceneDoor = jumpMap.AddComponent<Door>();
        sceneDoor.SetDoorNextScene(SceneName.JumpMap);
        sceneDoor.SetDoorType(DoorType.door);

        sceneDoor = maze.AddComponent<Door>();
        sceneDoor.SetDoorNextScene(SceneName.Maze);
        sceneDoor.SetDoorType(DoorType.door);

        sceneDoor = trap.AddComponent<Door>();
        sceneDoor.SetDoorNextScene(SceneName.Trap);
        sceneDoor.SetDoorType(DoorType.door);

        sceneDoor = treasure.AddComponent<Door>();
        sceneDoor.SetDoorNextScene(SceneName.Treasure);
        sceneDoor.SetDoorType(DoorType.door);

        initCamera = GameObject.Find("Init Camera");
        initCamera.SetActive(false);
        PlayableDirector finishDirector = initCamera.GetComponent<PlayableDirector>();
        finishDirector.stopped += OffCamera;
        sequence = 0;

    }

    void HallInitEnter()
    {
        if (fadeOutAfter != null) fadeOutAfter -= HallInitEnter;

        Transform playerTransform = GameManager.Instance.GetPlayer().transform;
        switch (sequence)
        {
            case 0:
                initCamera.SetActive(true);
                break;
            case 1:
                UIManager.Instance.StartDialogue(EventDialogue.SeeTheCat);
                break;
            default:
                break;
        }
        sequence++;
    }    

    void OffCamera(PlayableDirector pd)
    {
        initCamera.SetActive(false);
        UIManager.Instance.finishDialogue -= HallInitEnter;
        UIManager.Instance.StartDialogue(EventDialogue.SeeTheCat);
    }
}
