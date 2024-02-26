using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneName
{
    Intro = 0,
    StartMap,
    Hall,
    JumpMap,
    Maze,
    Quiz,
    Treasure,
    CantMoveScene = 800,  // 고장난 출입구 때 쓰일 열거형
    end = 999
}
public class Scene : MonoBehaviour
{
    // public static Scene instance;

    bool bLoad;
    private void Start()
    {
        // instance = this;
        bLoad = true;
    }

    ~Scene()
    {

    }

    protected virtual void Update()
    {
        if (bLoad)
        {
            bLoad = false;

            load();
        }

        fade(Time.deltaTime);
        iPoint point = mousePoint();
        for (int i = 0; i < kc.Length; i++)
        {
            if (Input.GetKeyDown(kc[i]))
            {
                keyDown |= kk[i];
                keyStat |= kk[i];
            }
            else if (Input.GetKeyUp(kc[i]))
                keyStat &= ~kk[i];
        }
    }

    int keyStat = key_none;
    int keyDown = key_none;
    public const int key_none = 0;
    public const int key_w = 1;
    public const int key_a = 2;
    public const int key_s = 4;
    public const int key_d = 8;
    public const int key_space = 256;
    KeyCode[] kc = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space };
    int[] kk = { key_w, key_a, key_s, key_d, key_space };
    public bool getKeyStat(int key) { return (keyStat & key) == key; }
    public bool GetKeyDown(int key) { return (keyDown & key) == key; }

    public RoomData room;

    iPoint mousePoint()
    {
        Vector3 v = Input.mousePosition;
        iPoint point = new iPoint(v.x, Screen.height - v.y);

        return point;
    }

    public virtual void load() { }
    public virtual void free() { }
    public void LoadPlayer(string spawnName = "spawn")
    {   // this function load Player on spawn Object Position
        PlayerPrefs.DeleteAll();
        Player player = GameManager.Instance.GetPlayer();
        if (player == null)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            obj.name = "Player";
            obj.transform.localScale *= 2;
            player = obj.AddComponent<Player>();
        }
        player.gameObject.layer = LayerMask.NameToLayer("Player");

        Transform spawnPos = GameObject.Find(spawnName).transform;
        Debug.Log("포지션 값 " + spawnPos.transform.position + ", " + player.transform.localPosition);
        player.transform.position = spawnPos.position;
        GameManager.Instance.SetPlayer(player);
    }

    [SerializeField] private string nameScene;
    [SerializeField] private float fadeDt = 0.5f;

    public void fade(float dt)
    {
        if (fadeDt == 0)
        {
            return;
        }
        GameManager.Instance.canInput = false;

        float alpha;

        if (fadeDt < 0.5f)
        {   // 어두워짐
            alpha = fadeDt / 0.5f;
            fadeDt += dt;
            if (fadeDt >= 0.5f)
                setSceneImmediately(nameScene);
        }
        else if (GameManager.Instance.isLoadScene)// && fadeDt < 1.0f)
        {   // 밝아짐
            alpha = 1.0f - (fadeDt - 0.5f) / 0.5f;
            fadeDt += dt;
            if (fadeDt >= 1.0f)
            {
                fadeDt = 0.0f;
                GameManager.Instance.canInput = true;
            }
        }
        else
        {
            alpha = 1.0f;
        }

        UIManager.Instance.GetFadeImage().color = new Color(0, 0, 0, alpha);
    }

    public void setScene(string name)
    {
        Debug.Log("현재 페이드 타임" + fadeDt);
        if (fadeDt != 0.0f)
            return;

        GameManager.Instance.isLoadScene = false;
        nameScene = name;
        fadeDt = 0.0001f;
    }

    private static GameObject goScene = null;
    public static void setSceneImmediately(string name)
    {
        GameObject go = new GameObject(name);
        System.Type type = System.Type.GetType(name);
        go.AddComponent(type);

        if (goScene != null)
        {
            if (GameManager.Instance.GetPlayer())
            {
                Destroy(GameManager.Instance.GetPlayer().gameObject);
                GameManager.Instance.SetPlayer(null);
            }
            GameObject.Destroy(goScene);
            Resources.UnloadUnusedAssets();
        }

        goScene = go;
        GameManager.Instance.curScene = go.GetComponent<Scene>();
    }

    public void LoadFinish()
    {
        GameManager.Instance.isLoadScene = true;
    }
}
