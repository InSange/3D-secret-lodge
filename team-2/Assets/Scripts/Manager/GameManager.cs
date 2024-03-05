using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager의 역할은 게임의 모든 데이터들을 관리하고
/// 뿌려주는 역할
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    private void Awake()
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

    public static GameManager Instance
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

    Dictionary<GameObject, int> roomState; // Room Clear?
    // 플레이어
    public Player player;   // Player

    [SerializeField] Dictionary<int, List<TextData>> textData;
    // 현재 씬
    public Scene curScene;  // Playing in Current User Scene

    // 상태 조건
    private bool isGetArtifact = false; // Room Artifact Get?
    private bool isNeedArtifact = false; // if Get Room Artifact Player can Escape
    [SerializeField] private bool isPause = false;
    public bool isLoadScene = true;   // Can Play? Scene Load Finish Check Flag
    public bool canInput;   // 입력 시스템 컨트롤
    public bool isPlaying;  // 현재 인게임 중인가?

    public bool getIsPause() {return isPause;}
    
    // 현재 플레이어 방오브젝트
    public GameObject playerInRoomOBJ;

    // 현재 먹은 유물 번호
    public int artifactNum = -1;    // 0 -> Maze Artifact, 1 -> Jump Artifact, 2 -> treasure Artifact, 3 -> Quiz Artifact 

    // 함정들
    // 1. Lava Set parameter
    public GameObject Jump_Map_lava;
    public Vector3 lavaStartPos;

    void Start()
    {
        Init();
        textData = CSVReader.LoadCSVData("File/Conversation");
        if (textData != null) Debug.Log("대화 데이터 로드 잘됨");

        foreach (var d in textData.Keys)
        {
            for (int i = 0; i < textData[d].Count; i++)
            {
                Debug.Log(textData[d][i].name + " : " + textData[d][i].text);
            }
        }

        new Main();
    }

    private void Init()
    {
        UIManager.Instance.CanvasSetting();

        isPause = false;
        Cursor.visible = true;
    }
  
    /// <param name="interactionOBJ"></param>
    public void SceneChange(SceneName nextScene)
    {
        switch (nextScene)
        {
            case SceneName.Intro:
                Debug.Log("인트로 이동");
                GameManager.Instance.curScene.setScene("Intro");
                break;
            case SceneName.StartMap:
                Debug.Log("시작 맵 이동");
                GameManager.Instance.curScene.setScene("StartMap");
                break;
            case SceneName.Hall:
                Debug.Log("홀 이동");
                GameManager.Instance.curScene.setScene("Hall");
                break;
            case SceneName.JumpMap:
                Debug.Log("점프맵 이동");
                GameManager.Instance.curScene.setScene("JumpMap");
                break;
            case SceneName.Maze:
                Debug.Log("미로 이동");
                GameManager.Instance.curScene.setScene("Maze");
                break;
            case SceneName.Trap:
                Debug.Log("퀴즈 이동");
                GameManager.Instance.curScene.setScene("Trap");
                break;
            case SceneName.Treasure:
                Debug.Log("보물찾기 이동");
                GameManager.Instance.curScene.setScene("Treasure");
                break;
            case SceneName.CantMoveScene:
                Debug.Log("고장난 문 이동");
                break;
            case SceneName.end:
                Debug.Log("마지막 열거형 이동");
                break;
            default:
                break;
        }
    }


    public void PauseFunc()
    {
        isPause = isPause ? false : true;

        UIManager.Instance.PauseGame(isPause);
    }

    [SerializeField] GameOverView overView;
    // 게임 오버
    public void GameOver(MonsterType monster)
    {
        try
        {
            overView.eventStart(monster);
        }
        catch (System.Exception)
        {
            curScene.setScene("Intro");
        }
    }

    public void SetPlayer(Player p)
    {
        player = p;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public List<TextData> GetDialugeData(int index = -1)
    {
        if (index == -1) return null;
        return (textData[index] != null ? textData[index] : null);
    }

    // GameData
    public static GameData data;
    public static string dataName = "data.sav";

    public static void LoadGameData()
    {
        byte[] bytes = FileIO.load(dataName);
        if (bytes != null) data = FileIO.bytes2struct<GameData>(bytes);
        else InitGameData();
    }

    public static void InitGameData()
    {
        data = new GameData();
        data.bgm = 1.0f;
        data.sfx = 1.0f;
        data.tutorial = false;
        data.visitedHall = false;
        data.clearJumpMap = false;
        data.clearMaze = false;
        data.clearTreasure = false;
        data.clearTrap = false;
        SaveGameData();
    }

    public static void SaveGameData()
    {
        byte[] bytes = FileIO.struct2bytes(data);
        FileIO.save(dataName, bytes);
    }

}