using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hall : Scene
{
    [SerializeField] GameObject map;

    // Door;
    [SerializeField] GameObject entrance;
    [SerializeField] GameObject jumpMap;
    [SerializeField] GameObject maze;
    [SerializeField] GameObject quiz;
    [SerializeField] GameObject treasure;

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
    }
}
