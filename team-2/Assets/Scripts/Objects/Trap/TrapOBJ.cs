using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapOBJ : MonoBehaviour
{
    public float dt;

    public virtual void Update()
    {
        dt += Time.deltaTime;
    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void InitSetting()
    {

    }

    public virtual void SecondPhaseSetting()
    {

    }
}
