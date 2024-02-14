using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    // �⺻ ĵ����
    [SerializeField] GameObject canvas;

    // ���� UI
    Image fadeImage;
    Image gameOverImage;
    GameObject treasurePanel;
    GameObject pausePanel;
    GameObject infoPanel;
    GameObject tutoPanel;
    GameObject talk_panel;
    Text talkText;
    Text nameText;

    // Intro Scene UI
    GameObject mainMenu;
    GameObject mainMenu_Panel;
    Button newGameButton;
    Button continueGameButton;
    Button gameExitButton;

    void DefaultUISetting()
    {
        RectTransform rect;

        // fade �̹���
        GameObject fadeOBJ = new GameObject();
        fadeOBJ.name = "FadeInOutImage";
        fadeOBJ.transform.parent = canvas.transform;
        fadeImage = fadeOBJ.AddComponent<Image>();
        fadeImage.color = new Color32(255, 255, 255, 255);
        rect = fadeOBJ.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);

        // ���ӿ��� �̹���
        GameObject gameOverOBJ = new GameObject();
        gameOverOBJ.name = "GameOverImage";
        gameOverOBJ.transform.parent = canvas.transform;
        gameOverImage = gameOverOBJ.AddComponent<Image>();
        gameOverImage.color = new Color32(255, 0, 0, 255);
        rect = gameOverOBJ.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);

        // Ʈ���� �̹��� UI
        infoPanel = Instantiate((GameObject)Resources.Load("Scene/CommonUI/TreasureImage"));
        infoPanel.name = "TreasureImage";
        infoPanel.transform.parent = canvas.transform;
        rect = infoPanel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);

        // ���� ���� UI
        infoPanel = Instantiate((GameObject)Resources.Load("Scene/CommonUI/Information_Panel"));
        infoPanel.name = "Information_Panel";
        infoPanel.transform.parent = canvas.transform;
        rect = infoPanel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);

        // �Ͻ� ���� UI
        pausePanel = Instantiate((GameObject)Resources.Load("Scene/CommonUI/Pause_Panel"));
        pausePanel.name = "Pause_Panel";
        pausePanel.transform.parent = canvas.transform;
        rect = pausePanel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);

        // ä�� UI
        talk_panel = Instantiate((GameObject)Resources.Load("Scene/CommonUI/Talk_Panel"));
        talk_panel.name = "Talk_Panel";
        talk_panel.transform.parent = canvas.transform;
        rect = talk_panel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, -520, 0);

        infoPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverImage.gameObject.SetActive(false);
        fadeImage.gameObject.SetActive(false);
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
        // �⺻ UI����
        DefaultUISetting();
    }
    
    public void MainMenuUI()
    {

    }

    public void InitGame()
    { 

    }

    public void PauseGame()
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

    public void PauseFunc(bool isPause)
    {
        if (isPause == true)
        {
            Cursor.visible = false;
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
        else
        {
            Cursor.visible = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
    }
}
