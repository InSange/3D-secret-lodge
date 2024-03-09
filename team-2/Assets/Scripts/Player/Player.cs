using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera m_Camera;    // 플레이어 카메라 (생성되면서 배치되어짐)
    private Ray ray;    // 카메라 중심을 기점으로 오브젝트 체크하기 위한 레이저 
    private RaycastHit hit; // 레이저와 접촉된 물체를 기록하는 변수
    Rigidbody rigid;        // 플레이어의 리지드바디.
    CapsuleCollider capSuleCollider;

    public bool live;

    // 이동 관련 변수
    [SerializeField] float hAxis, vAxis;     // 어느 방향으로 이동할 것인지 입력받아줄 변수.
    [SerializeField] bool shiftDown;
    [SerializeField] float playerSpeed = 5.0f;  // 플레이어의 기본 이동속도.
    [SerializeField] float jumpPower = 180.0f;    // 플레이어의 점프력
    [SerializeField] bool jDown;             // 점프 키
    [SerializeField] Vector3 movingWay;      // 플레이어가 나아갈 방향
    // 랜드 관련 변수
    [SerializeField] bool isGround;            // 땅에 착륙중인가? 
    [SerializeField] private Vector3 boxSize;   // 땅에 착륙중인지 체크하기 위한 박스
    [SerializeField] private float maxGroundDistance;   // 해당 박스를 놓을 위치
    [SerializeField] private Vector3 bodyBoxSize;
    [SerializeField] private float bodySize;
    [SerializeField] int bodylayerMask;
    // 상호작용 키들 ( 상호작용 및 esc 퍼즈 키 )
    bool iDown;             // 상호작용 키

    void Start()
    {
        this.gameObject.AddComponent<CapsuleCollider>();
        this.gameObject.AddComponent<Rigidbody>();
        // rigidBody Setting
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
        rigid.mass = 5f;
        // collider setting
        this.gameObject.tag = "Player";
        capSuleCollider = GetComponent<CapsuleCollider>();
        // camera setting
        GameObject camera = new GameObject("PlayerCamera");
        camera.transform.parent = this.transform;
        m_Camera = camera.AddComponent<Camera>();
        m_Camera.depth = 2;
        camera.AddComponent<Cammove>().player = this;
        camera.transform.localPosition = new Vector3(0f, 0.76f, 0f);
        camera.transform.localScale = new Vector3(1, 1, 1);

        isGround = false;
        live = true;
        boxSize = new Vector3(1, 0.5f, 1);
        maxGroundDistance = 2f;
        bodyBoxSize = new Vector3(2.5f, 3.0f, 2.5f);
        bodySize = -0.5f;
        bodylayerMask = 1 << LayerMask.NameToLayer("Wall");
    }

    void Update()
    {   if(live == false)
        {
            rigid.isKinematic = true;
            return;
        }
        if (GameManager.Instance.canInput && !GameManager.Instance.getIsPause() && live)
        {
            GetInput();
            Interaction();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.isPlaying)
        {
            GameManager.Instance.PauseFunc();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.canInput && !GameManager.Instance.getIsPause())
        {
            isGround = GroundCheck();
            Move();
            Jump();
        }
    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");//systemManager.isAction ? 0 : Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical"); //systemManager.isAction ? 0 : Input.GetAxis("Vertical");
        iDown = Input.GetKeyDown(KeyCode.E);//systemManager.isSelectInformation ? false : Input.GetKeyDown(KeyCode.E);
        shiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        if (Input.GetButtonDown("Jump")) jDown = true;
    }

    private void Move()
    {
        if(MoveCheck())
        {
            hAxis = 0;
            vAxis = 0;
        }
        movingWay = new Vector3(hAxis, 0, vAxis).normalized;
        float finalSpeed = (shiftDown) ? playerSpeed * 2 : playerSpeed;


        transform.Translate(movingWay * finalSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (jDown && isGround)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            jDown = false;
        }
    }

    void Interaction()
    {
        ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Physics.Raycast(ray, out hit, 3.0f);
        Debug.DrawRay(ray.origin, ray.direction * 3.0f, Color.red);
        if (hit.collider == null)
        {
            UIManager.Instance.SettingIcon(IconState.NONE);
            return;
        }
        // 삼중 대체할 만한 것이 있을까..?
        // 가독성 문제를 해결하기 위해 함수로 따로 빼주기.
        if (hit.collider.gameObject.CompareTag("Door"))
        {
            if (!iDown)
            {
                UIManager.Instance.SettingIcon(IconState.DoorOpen);
                return;
            }
            else
            {
                UIManager.Instance.SettingIcon(IconState.NONE);
            }
            Door doorInfo = hit.collider.gameObject.GetComponent<Door>();

            if (doorInfo.doorEvent != null) doorInfo.doorEvent();
            Debug.Log("Interaction + " + hit.collider.gameObject.name);

            switch (doorInfo.GetDoorType())
            {
                case DoorType.broken_door:
                    UIManager.Instance.StartDialogue(EventDialogue.BrookDoor);
                    break;
                case DoorType.need_talk:
                    UIManager.Instance.StartDialogue(EventDialogue.NeedTalkForOpenDoor);
                    break;
                case DoorType.door:
                    // 2중 스위치 검증.
                    GameManager.Instance.SceneChange(doorInfo.GetNextScene());
                    break;
                case DoorType.clear:
                    UIManager.Instance.StartDialogue(EventDialogue.ClearRoomDoor);
                    break;
                default:
                    break;
            }
        }
        else if(hit.collider.gameObject.CompareTag("NPC"))
        {
            if (!iDown)
            {
                UIManager.Instance.SettingIcon(IconState.Talk);
                return;
            }
            else
            {
                UIManager.Instance.SettingIcon(IconState.NONE);
            }
            NPC npc = hit.collider.gameObject.GetComponent<NPC>();
            npc.Talk();
            //UIManager.Instance.NPCTalk();
        }
        else if(hit.collider.gameObject.CompareTag("Artifact"))
        {
            if (!iDown)
            {
                UIManager.Instance.SettingIcon(IconState.Talk);
                return;
            }
            else
            {
                UIManager.Instance.SettingIcon(IconState.NONE);
            }
            Artifact artifact = hit.collider.gameObject.GetComponent<Artifact>();
            artifact.GetArtifact();
        }
        else if(hit.collider.gameObject.CompareTag("Treasure"))
        {
            if (!iDown)
            {
                UIManager.Instance.SettingIcon(IconState.Talk);
                return;
            }
            else
            {
                UIManager.Instance.SettingIcon(IconState.NONE);
            }
            TreasureBox box = hit.collider.gameObject.GetComponent<TreasureBox>();
            box.OpenBox();
        }
        else
        {
            UIManager.Instance.SettingIcon(IconState.NONE);
        }

    }

    bool GroundCheck()
    {
        return Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, maxGroundDistance);
    }

    bool MoveCheck()
    {
        // 움직임에 대한 로컬 벡터를 월드 벡터로 변환해준다.
        //movingWay = transform.TransformDirection(movingWay);
        // scope로 ray 충돌을 확인할 범위를 지정할 수 있다.
        float scope = 1.3f;

        // 플레이어의 머리, 가슴, 발 총 3군데에서 ray를 쏜다.
        List<Vector3> rayPositions = new List<Vector3>();
        //rayPositions.Add(transform.position + Vector3.up * 0.1f);
        rayPositions.Add(transform.position + Vector3.up * capSuleCollider.height * 0.5f);
        rayPositions.Add(transform.position + Vector3.up * capSuleCollider.height);

        Vector3 forward = transform.TransformDirection(movingWay); // new Vector3(movingWay.x * transform.forward.z, movingWay.y * transform.forward.z, movingWay.z * transform.forward.z);

        // 디버깅을 위해 ray를 화면에 그린다.
        foreach (Vector3 pos in rayPositions)
        {
            Debug.DrawRay(pos, forward * scope, Color.red);
        }

        // ray와 벽의 충돌을 확인한다.
        foreach (Vector3 pos in rayPositions)
        {
            if (Physics.Raycast(pos, forward, out RaycastHit hit, scope, bodylayerMask))
            {
                return true;
            }
        }
        return false;

        //return Physics.BoxCast(transform.position, bodyBoxSize, -transform.up, transform.rotation, bodySize);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position - transform.up * maxGroundDistance, boxSize);
    }
}