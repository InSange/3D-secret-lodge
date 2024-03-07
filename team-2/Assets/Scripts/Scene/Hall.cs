using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Hall : Scene
{
    [SerializeField] GameObject map;

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
            GameManager.Instance.fadeOutAfter += HallInitEnter;
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

        initCamera = GameObject.Find("Init Camera");
        initCamera.SetActive(false);
        PlayableDirector finishDirector = initCamera.GetComponent<PlayableDirector>();
        finishDirector.stopped += OffCamera;
        sequence = 0;

    }

    void HallInitEnter()
    {
        if (GameManager.Instance.fadeOutAfter != null) GameManager.Instance.fadeOutAfter -= HallInitEnter;

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
