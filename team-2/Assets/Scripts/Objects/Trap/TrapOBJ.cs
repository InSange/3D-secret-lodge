using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapOBJ : MonoBehaviour
{/// <summary>
/// 상위 클래스이다.
/// 상태 변수는 각기 다 달라서 하위 클래스에다가 선언을 해주었고
/// 공통적으로 사용하는 함수들만 따로 빼서 가상함수로 올려두었다.
/// </summary>
    public float dt;

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
