using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAxe : TrapOBJ
{/// <summary>
/// 매 시간마다 바뀌는 값들을 변수들로 등록해주었다.
/// </summary>
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
    /// <summary>
    /// 시간에 따라 도끼가 회전하는 방향을 조정해주었다!
    /// </summary>
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


        transform.localRotation = Quaternion.Euler(x, y, z);
    }

    public override void InitSetting()
    {
        base.InitSetting();
        isReturn = false;
        x = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player.live)
            {
                player.live = false;
                GameManager.Instance.GameOver(MonsterType.Axe);
            }
        }
    }
}
