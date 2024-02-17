using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIManager의 역할은 게임의 모든 UI를 관리하고
/// 뿌려주는 역할
/// </summary>
public class UIManager : MonoBehaviour
{
    //게임매니저의 인스턴스를 담는 전역변수(static 변수이지만 이해하기 쉽게 전역변수라고 하겠다).
    //이 게임 내에서 게임매니저 인스턴스는 이 instance에 담긴 녀석만 존재하게 할 것이다.
    //보안을 위해 private으로.
    private static UIManager instance = null;

    void Awake()
    {
        if(null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static UIManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    // 기본 캔버스
    [SerializeField] GameObject canvas;

    // 고정 UI
    Image fadeImage;
    Image gameOverImage;
    GameObject treasurePanel;
    GameObject pausePanel;
    GameObject infoPanel;
    GameObject tutoPanel;
    GameObject talk_panel;
    Text talkText;
    Text nameText;

    void DefaultUISetting()
    {
        // fade 이미지
        FadeImgSetting();

        // 게임오버 이미지
        GameOverImgSetting();

        // 트레저 이미지 UI
        TreasureImgSetting();

        // 던전 정보 UI
        DungeonInfoUISetting();

        // 일시 정지 UI
        PauseUISetting();

        // 채팅 UI
        TalkUISetting();
    }

    private void FadeImgSetting()
    {
        GameObject fadeOBJ = new GameObject();
        fadeOBJ.name = "FadeInOutImage";
        fadeOBJ.transform.SetParent(canvas.transform);
        fadeImage = fadeOBJ.AddComponent<Image>();
        fadeImage.color = new Color32(255, 255, 255, 0);
        fadeImage.raycastTarget = false;
        RectTransform rect = fadeOBJ.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
    }
    private void GameOverImgSetting()
    {
        GameObject gameOverOBJ = new GameObject();
        gameOverOBJ.name = "GameOverImage";
        gameOverOBJ.transform.SetParent(canvas.transform);
        gameOverImage = gameOverOBJ.AddComponent<Image>();
        gameOverImage.color = new Color32(255, 0, 0, 0);
        gameOverImage.raycastTarget = false;
        RectTransform rect = gameOverOBJ.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
    }
    private void TreasureImgSetting()
    {
        treasurePanel = Instantiate((GameObject)Resources.Load("Scene/CommonUI/TreasureImage"));
        treasurePanel.name = "TreasureImage";
        treasurePanel.transform.SetParent(canvas.transform);
        RectTransform rect = treasurePanel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        treasurePanel.SetActive(false);
    }


    private void DungeonInfoUISetting()
    {
        infoPanel = Instantiate((GameObject)Resources.Load("Scene/CommonUI/Information_Panel"));
        infoPanel.name = "Information_Panel";
        infoPanel.transform.SetParent(canvas.transform);
        RectTransform rect = infoPanel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);

        infoPanel.SetActive(false);
    }
    private void PauseUISetting()
    {
        pausePanel = Instantiate((GameObject)Resources.Load("Scene/CommonUI/Pause_Panel"));
        pausePanel.name = "Pause_Panel";
        pausePanel.transform.SetParent(canvas.transform);
        RectTransform rect = pausePanel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);

        Button mainMenuButton = GameObject.Find("MainMenu Button").GetComponent<Button>();
        mainMenuButton.onClick.AddListener(MainMenuButton);
        Button continueButton = GameObject.Find("Continue Button").GetComponent<Button>();
        continueButton.onClick.AddListener(ContinueButton);

        pausePanel.SetActive(false);
    }
    private void TalkUISetting()
    {
        talk_panel = Instantiate((GameObject)Resources.Load("Scene/CommonUI/Talk_Panel"));
        talk_panel.name = "Talk_Panel";
        talk_panel.transform.SetParent(canvas.transform);
        RectTransform rect = talk_panel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, -520, 0);

        talk_panel.SetActive(false);
    }
    public void CanvasSetting()
    {
        if (canvas) return;

        canvas = new GameObject("Canvas");
        Canvas c = canvas.AddComponent<Canvas>().GetComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler cs = canvas.AddComponent<CanvasScaler>().GetComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        canvas.AddComponent<GraphicRaycaster>();
        //canvas.transform.parent = this.transform;
        // 기본 UI세팅
        DefaultUISetting();
    }
    public void MainMenuButton()
    {
        GameManager.Instance.PauseFunc();
        //mainMenu_Panel.SetActive(true);
        Cursor.visible = true;
        GameManager.Instance.SceneChange(SceneName.Intro);
        Destroy(GameManager.Instance.player.gameObject);
    }

    private void ContinueButton()
    {
        GameManager.Instance.PauseFunc();
        //mainMenu_Panel.SetActive(false);
        Cursor.visible = false;
    }

    public void InitGame()
    { 

    }

    public void ContinueGame()
    {

    }

    public void RestartGame()
    {

    }

    public void StopGame()
    {

    }

    public void PauseGame(bool isPause)
    {
        if (isPause == true)
        {
            Cursor.visible = false;
            Time.timeScale = 0;
            pausePanel.SetActive(isPause);
        }
        else
        {
            Cursor.visible = true;
            Time.timeScale = 1;
            pausePanel.SetActive(isPause);
        }
    }

    public Image GetFadeImage()
    {
        return fadeImage;
    }
}
