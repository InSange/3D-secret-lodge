using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAxe : TrapOBJ
{
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float z;
    [SerializeField] float speed;
    [SerializeField] float upspeed;
    [SerializeField] bool isReturn;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        MoveAxe();
    }

    void MoveAxe()
    {
        if (isReturn == false)
        {
            x += Time.deltaTime * speed;

            if (x > 90.0f)
            {
                isReturn = true;
            }
        }
        else
        {
            x -= Time.deltaTime * upspeed;

            if (x < 0.0f)
            {
                isReturn = false;
            }
        }


        //transform.localRotation = Quaternion.Euler(x, transform.eulerAngles.y, 0);
        transform.localRotation = Quaternion.Euler(x, y, z);
    }

    public override void InitSetting()
    {
        base.InitSetting();
        isReturn = false;
        x = 0;
    }
}
