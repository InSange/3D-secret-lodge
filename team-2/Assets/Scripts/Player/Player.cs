using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera m_Camera;    // 플레이어 카메라 (생성되면서 배치되어짐)
    private Ray ray;    // 카메라 중심을 기점으로 오브젝트 체크하기 위한 레이저 
    private RaycastHit hit; // 레이저와 접촉된 물체를 기록하는 변수
    private bool m_Jump;

    public int life = 2; // 생명력.
    public bool live;

    float hAxis, vAxis;     // 어느 방향으로 이동할 것인지 입력받아줄 변수.
    float playerSpeed = 10;  // 플레이어의 기본 이동속도.
    float jumpPower = 8.0f;    // 플레이어의 점프력

    Vector3 groundOffset;
    bool isGround;            // 땅에 착륙중인가?

    bool jDown;             // 점프 키
    bool iDown;             // 상호작용 키
    bool pauseDown;         // pause Button

    public bool isLoading;  // 로딩중일때 플레이어 일시정지기능(움직임 및 점프 x).

    Vector3 movingWay;      // 플레이어가 나아갈 방향

    Rigidbody rigid;        // 플레이어의 리지드바디.
    CapsuleCollider capSuleCollider;

    public GameObject clickObject;  // 플레이어가 상호작용 할 오브젝트를 넣어줄 변수.

    //public SystemManager systemManager; // 시스템 매니저
    public RSP rsp;

    //public GameObject feildPointObj; // 필드이동에 사용되는 포인트 지점 체크하는 변수 

    void Start()
    {
        this.gameObject.AddComponent<CapsuleCollider>();
        this.gameObject.AddComponent<Rigidbody>();
        // rigidBody Setting
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
        // collider setting
        capSuleCollider = GetComponent<CapsuleCollider>();
        // camera setting
        GameObject camera = new GameObject("PlayerCamera");
        camera.transform.parent = this.transform;
        m_Camera = camera.AddComponent<Camera>();
        camera.AddComponent<Cammove>().player = this;
        camera.transform.localPosition = new Vector3(0f, 0.76f, 0f);
        camera.transform.localScale = new Vector3(1, 1, 1);

        groundOffset = new Vector3(0, -0.2f, 0);

        isGround = false;
        live = true;
    }

    void Update()
    {   // don't work Input System for Player that playing fade In/Out System now
        if (GameManager.Instance.canInput && !GameManager.Instance.getIsPause() && !isLoading)
        {
            GetInput();
            Move();
            Jump();
            Interaction();
        }
    }

    private void FixedUpdate()
    {
        isGround = GroundCheck();
    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");//systemManager.isAction ? 0 : Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical"); //systemManager.isAction ? 0 : Input.GetAxis("Vertical");
        jDown = Input.GetButtonDown("Jump");//systemManager.isAction ? false : Input.GetButtonDown("Jump");
        iDown = Input.GetKeyDown(KeyCode.E);//systemManager.isSelectInformation ? false : Input.GetKeyDown(KeyCode.E);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.PauseFunc();
        }
    }

    private void Move()
    {
        if (!isLoading)
        {
            movingWay = new Vector3(hAxis, 0, vAxis).normalized;

            transform.Translate(movingWay * playerSpeed * Time.deltaTime);
        }
    }

    void Jump()
    {
        if (jDown && isGround && !isLoading)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGround = true;
        }
    }

    void Interaction()
    {
        ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Physics.Raycast(ray, out hit, 3.0f);
        Debug.DrawRay(ray.origin, ray.direction * 3.0f, Color.red);
        if (hit.collider) Debug.Log("Check Obj " + hit.collider.gameObject.name);

        if (iDown && !isLoading && hit.collider)
        {
            if (hit.collider.gameObject.CompareTag("Door"))
            {
                Door doorInfo = hit.collider.gameObject.GetComponent<Door>();

                GameManager.Instance.SceneChange(doorInfo.GetNextScene());
                Debug.Log("Interaction + " + hit.collider.gameObject.name);
                //isLoading = true;
                //GameManager.Instance.Field_Change(clickObject);
            }
            else if(hit.collider.gameObject.CompareTag("NPC"))
            {
                Debug.Log("NPC Contact");
            }
            /*else if(clickObject.CompareTag("Artifact"))
            {
                GameManager.Instance.Get_Artifact(clickObject);
                clickObject = null;
            }
            else if(clickObject.CompareTag("Treasure"))
            {
                clickObject.GetComponent<TreasureBox>().OpenBox();
                clickObject = null;
            }
            else if(clickObject.CompareTag("Broken_Door"))
            {
               // systemManager.PlayerText(clickObject);
            }
            else if(clickObject.name == "BossRoom In Point")
            {
                Debug.Log(clickObject.name);
                GameManager.Instance.BossInRoom();
            }*/
        }
    }

    bool GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, capSuleCollider.height + Mathf.Abs(groundOffset.y)))
        {
            Debug.DrawRay(transform.position, Vector3.down * capSuleCollider.height + groundOffset, Color.red);
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*clickObject = other.gameObject;
        if(other.gameObject.CompareTag("Monster")&& live == true)
        {
            live = false;
            GameManager.Instance.GameOver();
        }
        else if(other.gameObject.CompareTag("Flame")&&live == true)
        {
            live = false;
            GameManager.Instance.GameOver();
        }
        else if(other.gameObject.name == "Paper" || other.gameObject.name == "Scissor" || other.gameObject.name == "Rock")
        {
            Debug.Log("내가 밟고 있는거 " + other.gameObject.name);
            rsp.SetPlayerRCP(other.gameObject.name);
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        clickObject = null;
        if (other.name == "StartMessage")
        {
            //systemManager.StartMessage();
            other.gameObject.SetActive(false);
            PlayerPrefs.SetInt("spawnPoint", 1);
        }
    }
}