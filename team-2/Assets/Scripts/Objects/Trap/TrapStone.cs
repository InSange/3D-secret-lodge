using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapStone : TrapOBJ
{
    [SerializeField] Vector3 startPos;
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float z;
    [SerializeField] float fallSpeed;
    [SerializeField] bool go;

    private void Start()
    {
        InitSetting();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Fall();
    }

    void Fall()
    {
        y -= fallSpeed * Time.deltaTime;
        transform.localPosition = new Vector3(x, y, z);
        if (y <= -20) y = startPos.y;
    }

    public override void InitSetting()
    {
        base.InitSetting();
        startPos = this.gameObject.transform.localPosition;
        x = startPos.x;
        y = startPos.y;
        z = startPos.z;
        fallSpeed = 20.0f;
        go = false;
    }

    public override void SecondPhaseSetting()
    {
        base.SecondPhaseSetting();
    }
}
