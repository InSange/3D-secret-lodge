using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EventDialogue
{
    StartMountain = 0,
    FoundLodge,
    InHall,
    SeeTheCat,
    TalkWithCat,
    TalkForInfo,
    LeftFirstRoom = 10,
    LeftSecondRoom,
    RightFirstRoom,
    RightSecondRoom,
    CenterRoom,
    BrookDoor = 800,
}

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
        if (null == instance)
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
            if (null == instance)
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
    [SerializeField] GameObject treasurePanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject infoPanel;
    [SerializeField] GameObject talk_panel;
    [SerializeField] Text talkText;
    [SerializeField] Text nameText;
    [SerializeField] Button mazeButton;
    [SerializeField] Button jumpButton;
    [SerializeField] Button treasureButton;
    [SerializeField] Button quizButton;
    [SerializeField] Button bossButton;

    // 시간 흐름 (텍스트 애니메이션 및 자동 스킵 기능)
    float dt;

    // 대화창에 관한 변수들
    [SerializeField] bool isDialogue;
    List<TextData> textDatas;
    [SerializeField] int dialogueIndex;
    [SerializeField] bool NPCMeet;

    // 대화창이 끝날 경우 실행시켜줄 델리게이트
    public delegate void DialogueEnd();
    public DialogueEnd finishDialogue;

    void Update()
    {
        if(isDialogue)
        {
            if(!GameManager.Instance.isPlaying)
            {
                isDialogue = false;
                talk_panel.SetActive(false);
            }
            dt += Time.deltaTime;
            if(dt > 1.0f)
            {
                dt = 0f;
                dialogueIndex++;
                ProgressDialogue();
            }
        }
    }

    // Make Canvas Object when to start game Awake time
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
        isDialogue = false;
        NPCMeet = false;
    }
    // All Default UI Load 
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
    // FadeImg Setting for Default UI
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
    // GameOverImg Setting for Default UI
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
    // TreasureUI Setting for Default UI
    private void TreasureImgSetting()
    {
        treasurePanel = Instantiate((GameObject)Resources.Load("Scene/CommonUI/TreasureImage"));
        treasurePanel.name = "TreasureImage";
        treasurePanel.transform.SetParent(canvas.transform);
        RectTransform rect = treasurePanel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        treasurePanel.SetActive(false);
    }
    // DungeonUI Setting for Default UI
    private void DungeonInfoUISetting()
    {
        infoPanel = Instantiate((GameObject)Resources.Load("Scene/CommonUI/Information_Panel"));
        infoPanel.name = "Information_Panel";
        infoPanel.transform.SetParent(canvas.transform);
        RectTransform rect = infoPanel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);

        mazeButton = GameObject.Find("Maze Button").GetComponent<Button>();
        jumpButton = GameObject.Find("Jump Button").GetComponent<Button>();
        treasureButton = GameObject.Find("Treasure Button").GetComponent<Button>();
        quizButton = GameObject.Find("Quiz Button").GetComponent<Button>();
        bossButton = GameObject.Find("Boss Button").GetComponent<Button>();

        mazeButton.onClick.AddListener(() => { StartDialogue(EventDialogue.LeftFirstRoom); CloseInformation(); });
        jumpButton.onClick.AddListener(() => { StartDialogue(EventDialogue.LeftSecondRoom); CloseInformation(); });
        treasureButton.onClick.AddListener(() => { StartDialogue(EventDialogue.RightFirstRoom); CloseInformation(); });
        quizButton.onClick.AddListener(() => { StartDialogue(EventDialogue.RightSecondRoom); CloseInformation(); });
        bossButton.onClick.AddListener(() => { StartDialogue(EventDialogue.CenterRoom); CloseInformation(); });

        infoPanel.SetActive(false);
    }
    // PuaseUI Setting for Default UI
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
    // TalkUI Setting for Default UI 
    private void TalkUISetting()
    {
        talk_panel = Instantiate((GameObject)Resources.Load("Scene/CommonUI/Talk_Panel"));
        talk_panel.name = "Talk_Panel";
        talk_panel.transform.SetParent(canvas.transform);
        RectTransform rect = talk_panel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, -520, 0);

        talkText = GameObject.Find("Talk_Text").GetComponent<Text>();
        nameText = GameObject.Find("Name_Text").GetComponent<Text>();

        talk_panel.SetActive(false);
    }
    // MainMenu Button in Pause Situation
    public void MainMenuButton()
    {
        GameManager.Instance.PauseFunc();
        //mainMenu_Panel.SetActive(true);
        Cursor.visible = true;
        GameManager.Instance.isPlaying = false;
        GameManager.Instance.SceneChange(SceneName.Intro);
        Destroy(GameManager.Instance.player.gameObject);
    }
    // Continue Button in Pause Situation
    private void ContinueButton()
    {
        GameManager.Instance.PauseFunc();
        //mainMenu_Panel.SetActive(false);
        Cursor.visible = false;
    }

    public void StartDialogue(EventDialogue e)
    {
        int eventIndex = ((int)e);
        textDatas = GameManager.Instance.GetDialugeData(eventIndex);

        if (textDatas == null) return;
        // 이벤트 데이터가 있을 시 플레이어를 조작할 수 없음!
        GameManager.Instance.canInput = false;  // 플레이어 조작 X
        dt = 0; // 시간 초기화
        dialogueIndex = 0; // 대사 구별 인덱스
        isDialogue = true;  // 대화 시작 구별 변수

        nameText.text = textDatas[dialogueIndex].name;
        talkText.text = textDatas[dialogueIndex].text;
        talk_panel.SetActive(true);
    }

    public void NPCTalk()
    {
        // 처음 만났을 때
        if (NPCMeet == false)
        {
            StartDialogue(EventDialogue.TalkWithCat);
            NPCMeet = true;
        }
        else// if(NPCMeet == true)
        {
            // 그 다음 만났을 때
            finishDialogue += OpenInformation;
            StartDialogue(EventDialogue.TalkForInfo);
        }
    }

    void OpenInformation()
    {
        finishDialogue -= OpenInformation;
        GameManager.Instance.canInput = false;
        infoPanel.SetActive(true);
    }

    void CloseInformation()
    {
        infoPanel.SetActive(false);
    }

    void ProgressDialogue()
    {   // 인덱스 값이 텍스트 개수와 동일하면 모든 대화는 끝난 것이다!
        if(dialogueIndex >= textDatas.Count)
        {
            isDialogue = false;
            talk_panel.SetActive(false);
            GameManager.Instance.canInput = true;
            if(finishDialogue != null) finishDialogue();
            return;
        }
        // 인덱스 값이 유효하면 다음 대화를 진행한다.
        nameText.text = textDatas[dialogueIndex].name;
        talkText.text = textDatas[dialogueIndex].text;
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
            Cursor.visible = true;
            Time.timeScale = 0;
            pausePanel.SetActive(isPause);
        }
        else
        {
            Cursor.visible = false;
            Time.timeScale = 1;
            pausePanel.SetActive(isPause);
        }
    }

    public Image GetFadeImage()
    {
        return fadeImage;
    }
}
