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

    Data data;
    Dictionary<GameObject, int> roomState; // Room Clear?
    // 플레이어
    public Player player;   // Player
    // 유저 데이터
    //public UserData playerData;   // Using PlayerData
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
    // 필드 및 스폰포인트.
    public Transform[] spawnPoints;

    public GameObject[] save_Artifacts;
    public GameObject[] maze_Spawn_point;
    public GameObject[] treasure_Spawn_point;
    public GameObject[] room_Artifacts;
    public GameObject[] rooms;
    public GameObject[] treasuresBox;
    
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

        /*roomState = new Dictionary<GameObject, int>();
        lavaStartPos = new Vector3(0, -37, 300);

        for (int i = 0; i < rooms.Length; i++)
        {
            roomState.Add(rooms[i], 0);
        }

        foreach (KeyValuePair<GameObject, int> pair in roomState)
        {
            Debug.Log(pair.Key.name + "," + pair.Value);
        }*/

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
            case SceneName.Trap:
                Debug.Log("점프맵 이동");
                GameManager.Instance.curScene.setScene("Trap");
                break;
            case SceneName.Maze:
                Debug.Log("미로 이동");
                GameManager.Instance.curScene.setScene("Maze");
                break;
            case SceneName.Quiz:
                Debug.Log("퀴즈 이동");
                GameManager.Instance.curScene.setScene("Quiz");
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

    public void Get_Artifact(GameObject artifact)
    {
        Debug.Log("얻은 유물 이름 : " + artifact.name);
        if(artifact.name == room_Artifacts[0].name)
        {
            room_Artifacts[0].SetActive(false);
            Maze_Room_Second_Phase();
            roomState[rooms[0]] = 1;
            artifactNum = 0;
        }
        else if(artifact.name == room_Artifacts[1].name)
        {
            room_Artifacts[1].SetActive(false);
            Jump_Room_Second_Phase();
            roomState[rooms[1]] = 1;
            artifactNum = 1;
        }
        else if(artifact.name == room_Artifacts[2].name)
        {
            room_Artifacts[2].SetActive(false);
            Treasure_Second_Phase();
            roomState[rooms[2]] = 1;
            artifactNum = 2;
        }
        else if(artifact.name == room_Artifacts[3].name)
        {
            room_Artifacts[3].SetActive(false);
            roomState[rooms[3]] = 1;
            artifactNum = 3;
        }
        if(isNeedArtifact)
        {
            isNeedArtifact = false;
        }
    }

    // 미로 2페이즈
    public void Maze_Room_Second_Phase()
    {
        for(int i = 0; i < maze_Spawn_point.Length; i++)
        {
            maze_Spawn_point[i].transform.GetChild(0).transform.localPosition = new Vector3(0,0,0);
            maze_Spawn_point[i].SetActive(true);
        }
    }

    // 미로 초기화!
    public void Maze_Room_First_Phase()
    {
        for (int i = 0; i < maze_Spawn_point.Length; i++)
        {
            maze_Spawn_point[i].SetActive(false);
        }
        if(artifactNum == 0)
        {
            room_Artifacts[artifactNum].SetActive(true);
            save_Artifacts[artifactNum].SetActive(false);
        }
        artifactNum = -1;
    }

    // 점프맵 2페이즈
    public void Jump_Room_Second_Phase()
    {
       // StartCoroutine(LavaON(Jump_Map_lava));
    }

    public void Jump_Room_First_Phase()
    {
        if(artifactNum == 1)
        {
            room_Artifacts[artifactNum].SetActive(true);
            save_Artifacts[artifactNum].SetActive(false);
        }
        Jump_Map_lava.transform.localPosition = lavaStartPos;
        artifactNum = -1;
    }

    public void Treasure_Second_Phase()
    {
        for(int i = 0; i < treasure_Spawn_point.Length; i++)
        {
            treasure_Spawn_point[i].transform.GetChild(0).transform.localPosition = new Vector3(0,0,0);
            treasure_Spawn_point[i].SetActive(true);
        }
    }

    public void Treasure_First_Phase()
    {
        for(int i = 0; i < treasure_Spawn_point.Length; i++)
        {
            treasure_Spawn_point[i].SetActive(false);
        }
        for(int i = 0; i < treasuresBox.Length; i++)
        {
            treasuresBox[i].GetComponent<TreasureBox>().reward.SetActive(false);
            treasuresBox[i].GetComponent<TreasureBox>().box.SetActive(true);
            treasuresBox[i].gameObject.GetComponent<BoxCollider>().enabled = true;        
        }
        if(artifactNum == 2) 
        {
            save_Artifacts[artifactNum].SetActive(false);
        }
        artifactNum = -1;
    }

    public void RSP_First_Phase()
    {
        if(artifactNum == 3)
        {
            room_Artifacts[artifactNum].SetActive(true);
            save_Artifacts[artifactNum].SetActive(false);
        }
        artifactNum = -1;
    }

    public void BossInRoom()
    {
        if(save_Artifacts[0].activeSelf == true && save_Artifacts[1].activeSelf == true && save_Artifacts[2].activeSelf == true && save_Artifacts[3].activeSelf == true)
        {
            Debug.Log("보스방 이동");
            SceneManager.LoadScene("Test 1");
        }
        else
        {
            Debug.Log("응 방 못들어가~");
        }
    }

    public void PauseFunc()
    {
        isPause = isPause ? false : true;

        UIManager.Instance.PauseGame(isPause);
    }


    // 게임 오버
    public void GameOver()
    {
        /*player.isLoading=true;
        StartCoroutine(GameOverLoadSetting());*/
    }

    /// <summary>
    ///  load to GameData . continue or new game
    /// </summary>
    public void Gameload()
    {
        
#if false   // dont use prev ver load to data

        /* system.talkId = 0;
         system.informationId = -1;
         system.contentNum = 0;
         system.informationNum = 0;

         if(PlayerPrefs.HasKey("spawnPoint"))
         {
             player.transform.position = spawnPoints[PlayerPrefs.GetInt("spawnPoint")].transform.position;
         }
         else
         {
             player.transform.position = spawnPoints[0].transform.position;
         }

         if(PlayerPrefs.HasKey("maze_state"))
         {
             roomState[rooms[0]] = 1;
             rooms[0].SetActive(false);
             save_Artifacts[0].SetActive(true);
         }
         else
         {
             roomState[rooms[0]] = 0;
             rooms[0].SetActive(true);
             save_Artifacts[0].SetActive(false);
             Maze_Room_First_Phase();
         }

         if(PlayerPrefs.HasKey("jump_state"))
         {
             roomState[rooms[1]] = 1;
             rooms[1].SetActive(false);
             save_Artifacts[1].SetActive(true);
         }
         else
         {
             roomState[rooms[1]] = 0;
             rooms[1].SetActive(true);
             save_Artifacts[1].SetActive(false);
             Jump_Room_First_Phase();
         }

         if(PlayerPrefs.HasKey("treasure_state"))
         {
             roomState[rooms[2]] = 1;
             rooms[2].SetActive(false);
             save_Artifacts[2].SetActive(true);
         }
         else
         {
             roomState[rooms[2]] = 0;
             rooms[2].SetActive(true);
             save_Artifacts[2].SetActive(false);
             Treasure_First_Phase();
         }

         if(PlayerPrefs.HasKey("RCP_state"))
         {
             roomState[rooms[3]] = 1;
             rooms[3].SetActive(false);
             save_Artifacts[3].SetActive(true);
         }
         else
         {
             roomState[rooms[3]] = 0;
             rooms[3].SetActive(true);
             save_Artifacts[3].SetActive(false);
             RSP_First_Phase();
         }*/
#endif
    }
    public void SetPlayer(Player p)
    {
        player = p;
    }

    public Player GetPlayer()
    {
        return player;
    }

#if false // dont use prev coroutine
    IEnumerator Current_Save()
    {
        yield return new WaitForSeconds(3.0f);
        playerInRoomOBJ.SetActive(false);
    }

    IEnumerator tpPos(Transform nextPos)
    {
        yield return new WaitForSeconds(2.0f);
        Vector3 spawnPos = new Vector3(nextPos.position.x, nextPos.position.y + 0.5f, nextPos.position.z);
        player.transform.position = spawnPos;
    }

    IEnumerator LavaON(GameObject lava)
    {
        /*yield return new WaitForSeconds(0.125f);
        if(lava.transform.position.y <= 50 && player.live == true)
        {
            lava.transform.Translate(Vector3.up * Time.deltaTime);
            Debug.Log(Jump_Map_lava.transform.position);
            StartCoroutine(LavaON(lava));
        }*/
    }

    IEnumerator GameOverLoadSetting()
    {
        systemManager.GetComponent<FadeInOut>().GameOverFadeFunc();
        yield return new WaitForSeconds(3.0f);

        player.transform.position = spawnPoints[1].transform.position;
        player.live = true;
        if(artifactNum == 0)
        {
            roomState[rooms[artifactNum]] = 0;
            Maze_Room_First_Phase();
        }
        else if(artifactNum == 1)
        {
            roomState[rooms[artifactNum]] = 0;
            Jump_Room_First_Phase();
        }
        else if(artifactNum == 2)
        {
            roomState[rooms[artifactNum]] = 0;
            Treasure_First_Phase();
        }
        else if(artifactNum == 3)
        {
            roomState[rooms[artifactNum]] = 0;
            RSP_First_Phase();
        }
    }
#endif

    public List<TextData> GetDialugeData(int index = -1)
    {
        if (index == -1) return null;
        return (textData[index] != null ? textData[index] : null);
    }
}