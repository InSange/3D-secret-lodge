using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMap : Scene
{
    [SerializeField] GameObject map;
    // Start is called before the first frame update
    void Start()
    {
        // background map instance
        map = Instantiate((GameObject)Resources.Load("Scene/StartMap/Start_MAP"));
        map.transform.SetParent(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
