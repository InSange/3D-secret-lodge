using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPipe : TrapOBJ
{
    [SerializeField] bool isUse;
    [SerializeField] float removeTime;

    private void Start()
    {
        InitSetting();
    }

    // Update is called once per frame
    void Update()
    {
        if(isUse)
        {
            dt += Time.deltaTime;
            if (dt >= removeTime) gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isUse = true;
            Debug.Log("플레이어 밟음!");
        }
    }

    public override void InitSetting()
    {
        base.InitSetting();
        isUse = false;
        dt = 0;
        removeTime = 5.0f;
    }
}
