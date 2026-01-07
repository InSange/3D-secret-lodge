using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 문의 타입은
/// 열리지 않는 문 = 0
/// 대화를 한 후 열리는 문(홀에서) = 1
/// 열리는 문 = 2
/// 이미 클리어한 문 = 3
/// 으로 분류된다.
/// </summary>
public enum DoorType
{
    broken_door = 0,
    need_talk,
    door,
    clear,
}

public class Door : MonoBehaviour
{   /// <summary>
    /// 각 문들은 전환이 가능한 씬들에 대한 정보를 지니고 있으며 
    /// 문의 상태가 Door일시 플레이어가 상호작용을 통해 씬 전환이 가능하다.
    /// 아래에는 문의 type을 저장해둘 변수이다.
    /// </summary>
    [SerializeField] SceneName nextScene;
    [SerializeField] DoorType type;
    /// <summary>
    /// 문을 열었을때 실행 시켜줄 이벤트를 달아줄 델리게이트 함수이다.
    /// </summary>
    public delegate void OpenDoor();
    public OpenDoor doorEvent;
    /// <summary>
    /// 전환해줄 씬을 저장해주는 함수이다.
    /// </summary>
    /// <param name="scene"></param>
    public void SetDoorNextScene(SceneName scene)
    {
        nextScene = scene;
    }
    /// <summary>
    /// 다음 씬을 알려줄 함수이다.
    /// </summary>
    /// <returns></returns>
    public SceneName GetNextScene()
    {
        return nextScene;
    }
    /// <summary>
    /// 문의 타입을 설정해줄 함수이다.
    /// </summary>
    /// <param name="t"></param>
    public void SetDoorType(DoorType t)
    {
        type = t;
    }
    /// <summary>
    /// 문의 타입을 반환해줄 함수이다.
    /// </summary>
    /// <returns></returns>
    public DoorType GetDoorType()
    {
        return type;
    }
}
