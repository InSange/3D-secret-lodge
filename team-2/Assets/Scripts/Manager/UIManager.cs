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
/// UIManager�� ������ ������ ��� UI�� �����ϰ�
/// �ѷ��ִ� ����
/// </summary>
public class UIManager : MonoBehaviour
{
    //���ӸŴ����� �ν��Ͻ��� ��� ��������(static ���������� �����ϱ� ���� ����������� �ϰڴ�).
    //�� ���� ������ ���ӸŴ��� �ν��Ͻ��� �� instance�� ��� �༮�� �����ϰ� �� ���̴�.
    //������ ���� private����.
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
    // �⺻ ĵ����
    [SerializeField] GameObject canvas;

    // ���� UI
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

    // �ð� �帧 (�ؽ�Ʈ �ִϸ��̼� �� �ڵ� ��ŵ ���)
    float dt;

    // ��ȭâ�� ���� ������
    [SerializeField] bool isDialogue;
    List<TextData> textDatas;
    [SerializeField] int dialogueIndex;
    [SerializeField] bool NPCMeet;

    // ��ȭâ�� ���� ��� ��������� ��������Ʈ
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
        // �⺻ UI����
        DefaultUISetting();
        isDialogue = false;
        NPCMeet = false;
    }
    // All Default UI Load 
    void DefaultUISetting()
    {
        // fade �̹���
        FadeImgSetting();

        // ���ӿ��� �̹���
        GameOverImgSetting();

        // Ʈ���� �̹��� UI
        TreasureImgSetting();

        // ���� ���� UI
        DungeonInfoUISetting();

        // �Ͻ� ���� UI
        PauseUISetting();

        // ä�� UI
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
        // �̺�Ʈ �����Ͱ� ���� �� �÷��̾ ������ �� ����!
        GameManager.Instance.canInput = false;  // �÷��̾� ���� X
        dt = 0; // �ð� �ʱ�ȭ
        dialogueIndex = 0; // ��� ���� �ε���
        isDialogue = true;  // ��ȭ ���� ���� ����

        nameText.text = textDatas[dialogueIndex].name;
        talkText.text = textDatas[dialogueIndex].text;
        talk_panel.SetActive(true);
    }

    public void NPCTalk()
    {
        // ó�� ������ ��
        if (NPCMeet == false)
        {
            StartDialogue(EventDialogue.TalkWithCat);
            NPCMeet = true;
        }
        else// if(NPCMeet == true)
        {
            // �� ���� ������ ��
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
    {   // �ε��� ���� �ؽ�Ʈ ������ �����ϸ� ��� ��ȭ�� ���� ���̴�!
        if(dialogueIndex >= textDatas.Count)
        {
            isDialogue = false;
            talk_panel.SetActive(false);
            GameManager.Instance.canInput = true;
            if(finishDialogue != null) finishDialogue();
            return;
        }
        // �ε��� ���� ��ȿ�ϸ� ���� ��ȭ�� �����Ѵ�.
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
