using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager�� ������ ������ ��� �����͵��� �����ϰ�
/// �ѷ��ִ� ����
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
    // �÷��̾�
    public Player player;   // Player

    [SerializeField] Dictionary<int, List<TextData>> textData;
    // ���� ��
    public Scene curScene;  // Playing in Current User Scene

    // ���� ����
    private bool isGetArtifact = false; // Room Artifact Get?
    private bool isNeedArtifact = false; // if Get Room Artifact Player can Escape
    [SerializeField] private bool isPause = false;
    public bool isLoadScene = true;   // Can Play? Scene Load Finish Check Flag
    public bool canInput;   // �Է� �ý��� ��Ʈ��
    public bool isPlaying;  // ���� �ΰ��� ���ΰ�?

    public bool getIsPause() {return isPause;}
    
    // ���� �÷��̾� �������Ʈ
    public GameObject playerInRoomOBJ;

    // ���� ���� ���� ��ȣ
    public int artifactNum = -1;    // 0 -> Maze Artifact, 1 -> Jump Artifact, 2 -> treasure Artifact, 3 -> Quiz Artifact 

    // ������
    // 1. Lava Set parameter
    public GameObject Jump_Map_lava;
    public Vector3 lavaStartPos;

    void Start()
    {
        Init();
        textData = CSVReader.LoadCSVData("File/Conversation");
        if (textData != null) Debug.Log("��ȭ ������ �ε� �ߵ�");

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
                Debug.Log("��Ʈ�� �̵�");
                GameManager.Instance.curScene.setScene("Intro");
                break;
            case SceneName.StartMap:
                Debug.Log("���� �� �̵�");
                GameManager.Instance.curScene.setScene("StartMap");
                break;
            case SceneName.Hall:
                Debug.Log("Ȧ �̵�");
                GameManager.Instance.curScene.setScene("Hall");
                break;
            case SceneName.JumpMap:
                Debug.Log("������ �̵�");
                GameManager.Instance.curScene.setScene("JumpMap");
                break;
            case SceneName.Maze:
                Debug.Log("�̷� �̵�");
                GameManager.Instance.curScene.setScene("Maze");
                break;
            case SceneName.Trap:
                Debug.Log("���� �̵�");
                GameManager.Instance.curScene.setScene("Trap");
                break;
            case SceneName.Treasure:
                Debug.Log("����ã�� �̵�");
                GameManager.Instance.curScene.setScene("Treasure");
                break;
            case SceneName.CantMoveScene:
                Debug.Log("���峭 �� �̵�");
                break;
            case SceneName.end:
                Debug.Log("������ ������ �̵�");
                break;
            default:
                break;
        }

        // dont use prev code Scene Change
#if true
        /*Objects OBJcomponent = interactionOBJ.GetComponent<Objects>();
        if(OBJcomponent.inRoom)
        {
            //isNeedArtifact = true;
            playerInRoomOBJ = OBJcomponent.RoomOBJ;
            if(roomState[playerInRoomOBJ] != 0)
            {
                player.isLoading = false;
                Debug.Log("Aready Clear!!");
                return;
            }
            isNeedArtifact = true;
            Transform nextPos = OBJcomponent.NextRoomPosition;
            systemManager.GetComponent<FadeInOut>().FadeFunc();
                
            StartCoroutine(tpPos(nextPos));
        }
        else
        {
            if(!isGetArtifact && !isNeedArtifact)  {    
                Transform nextPos = OBJcomponent.NextRoomPosition;
                systemManager.GetComponent<FadeInOut>().FadeFunc();
                    
                StartCoroutine(tpPos(nextPos));
            }
            else{
                systemManager.GetComponent<SystemManager>().narration(interactionOBJ);
                Debug.Log("Can't exit");
                player.isLoading = false;
                return;
            }
            if(artifactNum == 0) {
                save_Artifacts[artifactNum].SetActive(true);
                
                PlayerPrefs.SetInt("maze_state", 1);
            }
            else if(artifactNum == 1) {
                save_Artifacts[artifactNum].SetActive(true);
                
                PlayerPrefs.SetInt("jump_state", 1);
            }
            else if(artifactNum == 2) {
                save_Artifacts[artifactNum].SetActive(true);
                
                PlayerPrefs.SetInt("treasure_state", 1);
            }
            else if(artifactNum == 3) {
                save_Artifacts[artifactNum].SetActive(true);
                
                PlayerPrefs.SetInt("RCP_state", 1);
            }
            StartCoroutine(Current_Save());
        }*/
#endif
    }


    public void PauseFunc()
    {
        isPause = isPause ? false : true;

        UIManager.Instance.PauseGame(isPause);
    }


    // ���� ����
    public void GameOver()
    {
        /*player.isLoading=true;
        StartCoroutine(GameOverLoadSetting());*/
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