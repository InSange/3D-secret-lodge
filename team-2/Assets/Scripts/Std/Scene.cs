using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        for(int i = 0; i < kc.Length; i++)
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
        
        Transform spawnPos = GameObject.Find(spawnName).transform;
        Debug.Log("������ �� " + spawnPos.transform.position + ", " + player.transform.localPosition);
        player.transform.localPosition = spawnPos.localPosition;
        GameManager.Instance.SetPlayer(player);
    }

    private string nameScene;
    private float fadeDt = 0.5f;

    public void fade(float dt)
    {
        if (fadeDt == 0)
        {
            GameManager.Instance.canInput = true;
            return;
        }
        GameManager.Instance.canInput = false;

        float alpha;
        
        if (fadeDt < 0.5f)
        {   // ��ο���
            alpha = fadeDt / 0.5f;
            fadeDt += dt;
            if (fadeDt >= 0.5f)
                setSceneImmediately(nameScene);
        }
        else// if(fadeDt < 1.0f)
        {   // �����
            alpha = 1.0f - (fadeDt - 0.5f) / 0.5f;
            fadeDt += dt;
            if (fadeDt >= 1.0f)
                fadeDt = 0.0f;
        }

        UIManager.Instance.GetFadeImage().color = new Color(0, 0, 0, alpha);
    }

    public void setScene(string name)
    {
        Debug.Log("���� ���̵� Ÿ��" + fadeDt);
        if (fadeDt != 0.0f)
            return;

        nameScene = name;
        fadeDt = 0.0001f;
    }

    private static GameObject goScene = null;
    public static void setSceneImmediately(string name)
    {
        GameObject go = new GameObject(name);
        System.Type type = System.Type.GetType(name);
        go.AddComponent(type);

        if(goScene != null)
        {
            GameObject.Destroy(goScene);
            Resources.UnloadUnusedAssets();
            if(GameManager.Instance.GetPlayer())
            {
                GameManager.Instance.SetPlayer(null);
            }
        }

        goScene = go;
        GameManager.Instance.curScene = go.GetComponent<Scene>();
    }
}
