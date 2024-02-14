using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : Scene
{
    GameObject mainMenu;
    GameObject introCanvas;
    GameObject mainMenu_Panel;
    Button newGameButton;
    Button continueGameButton;
    Button gameExitButton;

    void Start()
    {
        // mainMenu prefab setting
        mainMenu = Instantiate((GameObject)Resources.Load("Scene/Intro/MainMenu"));
        mainMenu.transform.parent = this.transform;

        // Intro Canvas make
        introCanvas = new GameObject("Canvas");
        Canvas canvas = introCanvas.AddComponent<Canvas>().GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler cs = introCanvas.AddComponent<CanvasScaler>().GetComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        introCanvas.AddComponent<GraphicRaycaster>();
        introCanvas.transform.SetParent(this.transform);

        // Intro UI setting
        mainMenu_Panel = Instantiate((GameObject)Resources.Load("Scene/Intro/MainMenu_Panel"));
        mainMenu_Panel.transform.SetParent(introCanvas.transform);
        RectTransform rect = mainMenu_Panel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        newGameButton = GameObject.Find("NewGame Button").GetComponent<Button>();
        continueGameButton = GameObject.Find("ContinueGame Button").GetComponent<Button>();
        gameExitButton = GameObject.Find("GameExit Button").GetComponent<Button>();

        mainMenu.SetActive(true);
        mainMenu_Panel.SetActive(true);

        newGameButton.onClick.AddListener(NewGameButton);
        continueGameButton.onClick.AddListener(ContinueGameButton);
        gameExitButton.onClick.AddListener(ExitGameButton);
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

    void NewGameButton()
    {
        setScene("StartMap");
    }

    void ContinueGameButton()
    {
        mainMenu_Panel.SetActive(false);
        GameManager.instance.Gameload();
        mainMenu.gameObject.SetActive(false);
        Cursor.visible = false;
    }

    void ExitGameButton()
    {
        Application.Quit();
    }
}
