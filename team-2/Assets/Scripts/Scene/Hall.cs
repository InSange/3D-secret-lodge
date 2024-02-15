using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hall : Scene
{
    [SerializeField] GameObject map;

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
    }
}
