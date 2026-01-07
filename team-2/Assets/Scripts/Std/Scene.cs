using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 만들고자하는 씬을 분류하기 위해 열거형을 선언해줬다.
/// </summary>
public enum SceneName
{
    Intro = 0,
    StartMap,
    Hall,
    JumpMap,
    Maze,
    Treasure,
    Trap,
    CantMoveScene = 800,  // 고장난 출입구 때 쓰일 열거형
    end = 999
}
public class Scene : MonoBehaviour
{
    /// <summary>
    /// 씬 세팅을 위한 불 변수
    /// Update에서 딱 한번 실행해준다.
    /// </summary>
    bool bLoad;

    private void Start()
    {
        // instance = this;
        bLoad = true;
    }

    ~Scene()
    {

    }

    protected virtual void Update()
    {   // 초기 데이터 로드
        if (bLoad)
        {
            bLoad = false;

            load();
        }
        // 페이드 인 아웃
        fade(Time.deltaTime);
        // 여기서 쓰지 않는 키입력 확인 구문.
        // 사용자가 키 환경세팅을 직접 설정해줄 수 있게 해준다.
        iPoint point = mousePoint();
        for (int i = 0; i < kc.Length; i++)
        {
            if (Input.GetKeyDown(kc[i]))
            {
                keyDown |= kk[i];
                keyStat |= kk[i];
            }
            else if (Input.GetKeyUp(kc[i]))
                keyStat &= ~kk[i];
        }
    }
    /// <summary>
    /// 우리가 알고있는 키 바인딩 코드이다!
    /// 해당 방법도 직접 써보면 좋지만 Unity 프로젝트 세팅안에서 
    /// 제공해주는 키 바인딩을 적용했음.
    /// </summary>
    int keyStat = key_none;
    int keyDown = key_none;
    public const int key_none = 0;
    public const int key_w = 1;
    public const int key_a = 2;
    public const int key_s = 4;
    public const int key_d = 8;
    public const int key_space = 256;
    KeyCode[] kc = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space };
    int[] kk = { key_w, key_a, key_s, key_d, key_space };
    public bool getKeyStat(int key) { return (keyStat & key) == key; }
    public bool GetKeyDown(int key) { return (keyDown & key) == key; }

    /// <summary>
    /// 각 씬마다 씬에 사용되는 데이터(오브젝트, 기능)들에
    /// 접근할 수 있도록 씬마다 공통적으로 지닌다. 
    /// 씬을 구성하는 맵 오브젝트이다!
    /// </summary>
    public RoomData room;
    public GameObject map;
    
    iPoint mousePoint()
    {
        Vector3 v = Input.mousePosition;
        iPoint point = new iPoint(v.x, Screen.height - v.y);

        return point;
    }
    /// <summary>
    /// 씬 로드시 최초 구성을 로드하고
    /// 메모리 제거할 때 쓰는 load, free함수.
    /// 각 씬마다 최초 구성이 다를 수 있기 때문에 가상으로 설정
    /// </summary>
    public virtual void load() { }
    public virtual void free() { }
    /// <summary>
    /// 플레이어를 씬에 배치시켜 주기 위한 함수이다.
    /// 매개변수로 spawnName이 존재하며
    /// 해당 변수이름을 가진 오브젝트를 찾아 해당 오브젝트 포지션에 위치시킨다.
    /// </summary>
    /// <param name="spawnName"></param>
    public void LoadPlayer(string spawnName = "spawn")
    {   // this function load Player on spawn Object Position
        // 플레이어를 불러오는데 존재하는 오브젝트가 없을 시 새로 오브젝트를 만든다!
        // 플레이어 최초 구성
        Player player = GameManager.Instance.GetPlayer();
        if (player == null)
        {   // 캡슐 오브젝트와 이름, 크기, Player컴포넌트를 설정해준다.
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            obj.name = "Player";
            obj.transform.localScale *= 2;
            player = obj.AddComponent<Player>();
        }
        // 플레이어와 다른 오브젝트를 구분하기 위한 Layer 값을 설정해준다.
        player.gameObject.layer = LayerMask.NameToLayer("Player");
        // spawnName 오브젝트를 찾아서 해당 위치로 플레이어를 세팅해준다.
        Transform spawnPos = GameObject.Find(spawnName).transform;
        player.transform.position = spawnPos.position;
        // 만들어진 플레이어는 게임 매니저에서 관리를 하며 다른 스크립트에서 접근가능하게 한다.
        GameManager.Instance.SetPlayer(player);
    }
    /// <summary>
    /// 변경하고자하는 씬의 이름을 저장해주는 nameScene 변수
    /// 페이드 인 아웃을 할때 사용할 실수형 fadeDt 변수
    /// </summary>
    [SerializeField] private string nameScene;
    [SerializeField] private float fadeDt = 0.5f;
    /// <summary>
    /// 씬에서 다른 씬으로 전환할때 화면이 검게 페이드 인이되고 아웃이된다.
    /// </summary>
    /// <param name="dt"></param>
    public void fade(float dt)
    {
        if (fadeDt == 0)
        {   // 0초면 페이드인/아웃을 실행하는게 아닌 반환.
            return;
        }
        // 페이드 인 아웃 중일때는 플레이어를 움직일 수 없도록 제한한다.
        GameManager.Instance.canInput = false;
        // 페이드 인 아웃 이미지의 알파(투명도)값을 조정해주기 위한 변수.
        float alpha;
        // 0.5초 보다 작으면.
        if (fadeDt < 0.5f)
        {   // 어두워짐
            alpha = fadeDt / 0.5f;
            fadeDt += dt;
            if (fadeDt >= 0.5f)
            {   // 0.5초 보다 크면(화면이 검은색 이미지로 가려지면)
                // 바로 다음 씬으로 전환된다.
                // 페이드인이 끝나고 실행되어야 할 이벤트가 있으면 실행해준다!
                setSceneImmediately(nameScene);
                if (GameManager.Instance.fadeInFinish != null) GameManager.Instance.fadeInFinish();
            }
        }   // 씬이 로딩이 끝났을때
        else if (GameManager.Instance.isLoadScene)// && fadeDt < 1.0f)
        {   // 밝아짐
            alpha = 1.0f - (fadeDt - 0.5f) / 0.5f;
            fadeDt += dt;
            if (fadeDt >= 1.0f)
            {   // 1초보다 크면 0초로 초기화시켜줌 (0초는 return으로 종료함.)
                // 플레이어 조종이 가능해지고 페이드 아웃이 끝나고 실행해야할 이벤트가 있으면 실행.
                fadeDt = 0.0f;
                GameManager.Instance.canInput = true;
                if (GameManager.Instance.fadeOutAfter != null) GameManager.Instance.fadeOutAfter();
            }
        }   // 씬이 로딩중일 때 (초기세팅중.)
        else
        {   // 리소스가 로딩되는데에 시간이 걸린다..!
            // 해당 시간동안에는 검은 화면만 보여준다.
            alpha = 1.0f;
        }
        // alpha값에 따라 검은 이미지의 투명도를 조정해준다.
        // 페이드 인일때는 0.5를 나눠줌으로써 0 ~ 0.5 동안에는 점점 검해지고
        // 페이드 아웃일때는 0.5를 빼고 0.5를 나눠줌으로 써 0.5 ~ 1 동안에는 점점 투명해진다.
        UIManager.Instance.GetFadeImage().color = new Color(0, 0, 0, alpha);
    }
    /// <summary>
    /// 씬을 전환할때 사용하는 함수이다.
    /// </summary>
    /// <param name="name"></param>
    public void setScene(string name)
    {   // fadeDt가 0이 아니면 아직 페이드 인/아웃이 진행중이기에 씬 전환이 이뤄지면 안된다!
        if (fadeDt != 0.0f)
            return;
        // 로딩 되었는지 확인하기 위한 변수 isLoadScene을 false로 바꾸고 전환할 씬의 이름을 저장해준다.
        // fadeDt 값을 0.0001초로 설정해둠으로써 fade함수를 return값에서 페이드 인/아웃 활동을 하게끔 해준다.
        GameManager.Instance.isLoadScene = false;
        nameScene = name;
        fadeDt = 0.0001f;
    }
    // 바꾼 씬 오브젝트
    private static GameObject goScene = null;
    /// <summary>
    /// name이름을 가진 오브젝트를 만들고 해당 이름의 컴포넌트를 추가해준다.
    /// </summary>
    /// <param name="name"></param>
    public static void setSceneImmediately(string name)
    {   // 오브젝트 name 생성 후 name 컴포넌트 추가(씬의 정보를 담은)
        GameObject go = new GameObject(name);
        System.Type type = System.Type.GetType(name);
        go.AddComponent(type);

        if (goScene != null)
        {   // 만들어진 플레이어가 있으면 파괴한뒤 새로 만들 수 있게 초기화 시켜준다.
            if (GameManager.Instance.GetPlayer())
            {
                Destroy(GameManager.Instance.GetPlayer().gameObject);
                GameManager.Instance.SetPlayer(null);
            }
            GameObject.Destroy(goScene);    // 기존 씬 제거
            Resources.UnloadUnusedAssets();
        }
        // 기존 씬 제거 후 새로 만든 go 씬을 현재 씬으로 설정해준다.
        goScene = go;
        GameManager.Instance.curScene = go.GetComponent<Scene>();
    }
    /// <summary>
    /// 씬 데이터소스들을 모두 세팅이 되었다면 씬에게 알려주기 위한 함수.
    /// </summary>
    public void LoadFinish()
    {   
        GameManager.Instance.isLoadScene = true;
    }
}
