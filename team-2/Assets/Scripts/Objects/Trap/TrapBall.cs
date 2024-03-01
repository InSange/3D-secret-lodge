using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBall : TrapOBJ
{
    [SerializeField] float speed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float z;
    [SerializeField] bool isMove;

    private void Start()
    {
        InitSetting();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isMove)
        {
            Move();
        }
    }

    void Move()
    {
        transform.Rotate(Vector3.back * Time.deltaTime * rotateSpeed);
        x += speed * Time.deltaTime;
        transform.localPosition = new Vector3(x, y, z);
    }

    public override void InitSetting()
    {
        base.InitSetting();
        Vector3 startPos = this.gameObject.transform.localPosition;
        x = startPos.x;
        y = startPos.y;
        z = startPos.z;
        speed = 2.0f;
        rotateSpeed = 30.0f;
        isMove = false;
    }

    public override void SecondPhaseSetting()
    {
        base.SecondPhaseSetting();
        isMove = true;
    }
}
