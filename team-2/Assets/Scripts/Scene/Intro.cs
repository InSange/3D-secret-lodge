using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 시작하면 나오는 메인화면을 구성하는 인트로 씬이다.
/// </summary>
public class Intro : Scene
{   /// <summary>
    /// 메인 메뉴를 구성하는 UI들과 각 버튼들의 기능들을 부여받기 위한 변수들이다.
    /// </summary>
    GameObject introCanvas;
    GameObject mainMenu;
    GameObject mainMenu_Panel;
    Button newGameButton;
    Button continueGameButton;
    Button gameExitButton;

    void Start()
    {   
        // background map instance
        // 배경이되는 맵으로써 리소스를 가져와준다.
        map = Instantiate((GameObject)Resources.Load("Scene/StartMap/Start_MAP"));
        map.transform.SetParent(this.transform);
        // 메인 메뉴 카메라세팅
        mainMenu = Instantiate((GameObject)Resources.Load("Scene/Intro/MainMenu"));
        mainMenu.transform.parent = this.transform;

        // Intro Canvas make
        // 인트로에 사용될 캔버스(UI들을 담아두기 위한 곳)을 추가해준다.
        // 공통 UI를 관리하는 UI Manager와 달리 인트로에서만 사용하는 일회성이라
        // 따로 캔버스를 만들어서 직접 관리해주도록 설계하였다.
        introCanvas = new GameObject("Canvas");
        Canvas canvas = introCanvas.AddComponent<Canvas>().GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler cs = introCanvas.AddComponent<CanvasScaler>().GetComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        introCanvas.AddComponent<GraphicRaycaster>();
        introCanvas.transform.SetParent(this.transform);

        // Intro UI setting
        // 인트로에서만 사용하는 UI들을 리소스폴더에서 불러와 미리 만둘어둔 Canvas에다가 넣어주었다.
        mainMenu_Panel = Instantiate((GameObject)Resources.Load("Scene/Intro/MainMenu_Panel"));
        mainMenu_Panel.transform.SetParent(introCanvas.transform);
        RectTransform rect = mainMenu_Panel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        newGameButton = GameObject.Find("NewGame Button").GetComponent<Button>();
        continueGameButton = GameObject.Find("ContinueGame Button").GetComponent<Button>();
        gameExitButton = GameObject.Find("GameExit Button").GetComponent<Button>();

        mainMenu_Panel.SetActive(true);
        // 판넬에 존재하는 각각의 버튼들에 기능들을 넣어준다.
        newGameButton.onClick.AddListener(NewGameButton);
        continueGameButton.onClick.AddListener(ContinueGameButton);
        gameExitButton.onClick.AddListener(ExitGameButton);
        // 모든 데이터들이 로드되었다면 끝났다고 알려주어 페이드 아웃을 진행한다.
        LoadFinish();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void load()
    {
        base.load();
    }

    public override void free()
    {
        base.free();
    }
    /// <summary>
    /// 새로운 게임 시작버튼이다.
    /// 게임 매니저에서 데이터를 초기화하고 초기맵인 StartMap으로 씬전환이 이루어진다.
    /// </summary>
    void NewGameButton()
    {
        GameManager.InitGameData();
        GameManager.Instance.SceneChange(SceneName.StartMap);
        GameManager.Instance.isPlaying = true;
    }
    /// <summary>
    /// 이어서 게임하기 버튼이다.
    /// 이전에 플레이했던 게임 데이터를 불러와서 튜토리얼(StartMap)을 플레이 했다면 Hall로
    /// 아니라면 StartMap으로 다시 튜토리얼을 진행한다.
    /// </summary>
    void ContinueGameButton()
    {
        GameManager.LoadGameData();
        if (GameManager.data.tutorial == false)
        {
            GameManager.Instance.SceneChange(SceneName.StartMap);
        }
        else
        {
            Cursor.visible = false;
            GameManager.Instance.SceneChange(SceneName.Hall);
        }
        GameManager.Instance.isPlaying = true;
    }
    /// <summary>
    /// 게임 종료!
    /// </summary>
    void ExitGameButton()
    {
        Application.Quit();
    }
}
