using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMap : Scene
{
    GameObject startMap_Canvas;
    [SerializeField] GameObject map;
    [SerializeField] Transform spawn;
    [SerializeField] GameObject door;
    [SerializeField] GameObject tutoPanel;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    /// <summary>
    /// Load object and data to StartMap
    /// </summary>
    private void Init()
    {
        LoadMap();
        LoadUI();

        PlayerPrefs.DeleteAll();
        LoadPlayer();

        // Player move restrict to message
        GameManager.Instance.player.isLoading = true;
        LoadFinish();
    }

    void LoadMap()
    {   // Load BackGround Map
        map = Instantiate((GameObject)Resources.Load("Scene/StartMap/Start_MAP"));
        map.transform.SetParent(this.transform);
    }

    void LoadUI()
    {
        // ±âº» Äµ¹ö½º ¼¼ÆÃ
        startMap_Canvas = new GameObject("Canvas");
        Canvas c = startMap_Canvas.AddComponent<Canvas>().GetComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler cs = startMap_Canvas.AddComponent<CanvasScaler>().GetComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        startMap_Canvas.AddComponent<GraphicRaycaster>();
        startMap_Canvas.transform.parent = this.transform;
        // ½ÃÀÛ ¸Ê UI
        tutoPanel = Instantiate((GameObject)Resources.Load("Scene/StartMap/Tutorial_Panel"));
        tutoPanel.transform.parent = startMap_Canvas.transform;
        RectTransform rect = tutoPanel.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0, 0, 0);
        Button tutoButton = GameObject.Find("Start_Button").GetComponent<Button>();
        tutoButton.onClick.AddListener(TutorialButton);

        tutoPanel.SetActive(true);
    }

    void TutorialButton()
    {
        tutoPanel.SetActive(false);
        Cursor.visible = false;
        GameManager.Instance.player.isLoading = false;
        // TutorialMessage();
    }
}
