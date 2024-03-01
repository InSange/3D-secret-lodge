using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMovingFloor : TrapOBJ
{
    [SerializeField] Vector3 startPos;
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float z;
    [SerializeField] float speed;
    [SerializeField] bool turn;
    [SerializeField] float turnPoint1;
    [SerializeField] float turnPoint2;

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
        if (turn)
        {
            z -= speed * Time.deltaTime;
            if (z < turnPoint1) turn = false;
        }
        else
        {
            z += speed * Time.deltaTime;
            if (z > turnPoint2) turn = true;
        }
        transform.localPosition = new Vector3(x, y, z);
    }

    public override void InitSetting()
    {
        base.InitSetting();
        startPos = this.gameObject.transform.localPosition;
        x = startPos.x;
        y = startPos.y;
        z = startPos.z;
        speed = 1.0f;
        turn = false;
    }

    public override void SecondPhaseSetting()
    {
        base.SecondPhaseSetting();
    }
}
