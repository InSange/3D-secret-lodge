using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.AI;
//using UnityEngine.UIElements;

public class BossPlayer : MonoBehaviour
{
    private GameObject ending;
    public enum playState//�÷��̾� ���� ����
    {
        Normal,
        Stun
    }
    public float hp = 15f;
    public int attackPower = 3;//?��?��?��?��?�� 공격?��
    private float hAxis, vAxis;
    private bool jDown;
    private bool iDown;
    public Image redScreen;
    public float playerSpeed;

    public bool isAttacked = false;
    private bool isDodge;

    private Vector3 jumpVector;
    private Vector3 movingWay;

    private Transform bossWay;
    private Animator anim;
    private CharacterController playerController;
    public Transform attackPoint; //?��?��?��?��?�� 공격?�� ?���??�� ?���?

    
    
    public bool isLoading;  // �ε����϶� �÷��̾� �Ͻ��������?(������ �� ���� x).
    
    GameObject clickObject;  // �÷��̾ ��ȣ�ۿ� �� ������Ʈ�� �־��� ����.

    public GameManager gameManager; // ���ӸŴ���
    public playState p_State;
    public Transform enemypos;
    float currentTime;// 공격 ?��?��?��?�� �??��
    float attackDelay = 0.5f;// 공격 �? ?��?��?���?
    
    private BoxCollider boxTest;

    private GameObject boss;
    // Start is called before the first frame update
    private void Awake()
    {
        ending = GameObject.Find("Canvas");
        boss = GameObject.Find("Boss");
        boxTest = boss.GetComponent<BoxCollider>();
        playerController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        bossWay = GameObject.Find("Boss").transform;
        p_State = playState.Normal;//�÷��̾� ���¸� �⺻ ���·� 
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(p_State == playState.Stun)//?��?��(?�� ?��치로 ?��?��?��?�� ?��?��?��기기) ?��?���? ?���? ?��?��
        {
            Vector3 way = (enemypos.position - transform.position).normalized;//?��?��?��?���? ?��겨오�? ?��?�� 벡터�? 방향?�� 계산?���?
            transform.Translate(way * 2 * Time.deltaTime, Space.World);//?��?�� ?��?�� ?��?�� 값만?�� ?��겨온?��.
            return;//?��?��?��?���? ?��?�� ?��?���? ?��?�� ?��?���? ?��?��?��?���? 리턴->?��?��?��?�� ???직임 ?��?��?�� 받�??못함
        }
        Attack();//공격 처리 ?��?��
        Gravity();
        GetInput();
        Move();
        Dodge();
        Interaction();
    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        iDown = Input.GetKeyDown(KeyCode.E);
        jDown = Input.GetKeyDown(KeyCode.Space);
    }

    private void Attack()///공격 ?��?�� ?��?��
    {
        currentTime += Time.deltaTime;//공격 쿨�???�� 
        if (currentTime > attackDelay)//공격 쿨�???��?�� 차면 공격 �??��
        {
            if (Input.GetMouseButtonDown(0))//공격 쿨�???��?�� �? ?��?��?��?�� 마우?�� 좌클�? ?�� 공격
            {
                print("공격 ?��?��");
                Ray ray = new Ray(attackPoint.position, attackPoint.forward);
                Debug.DrawRay(ray.origin, ray.direction * 3f, Color.blue);
                RaycastHit hit;
                currentTime = 0;//공격 쿨�???�� ?��리기
                if (Physics.Raycast(ray, out hit, 3f)) //1,5.길이?�� 짧�?? ?��?��(근접 공격)?�� 보스�? 맞으�? 보스?�� ?��미�?? 처리 ?��?���? 불러??? 공격 
                {
                    if (hit.collider.CompareTag("Test"))///////////////////////////////보스 ?��그이�?
                    {
                        bossWay.GetComponent<BossScript>().HitEnemy(attackPower);
                        print("보스 공격");
                    }
                }
            }
        }

    }
    private void Move()
    {
        if(isLoading) return;
        
        movingWay = new Vector3(hAxis, 0, vAxis).normalized;
        movingWay = transform.TransformDirection(movingWay);
        if (isAttacked)
        {
            movingWay = (bossWay.transform.position - gameObject.transform.position).normalized;
            //movingWay = transform.TransformDirection(movingWay);
            playerController.Move(-movingWay * 100 * Time.deltaTime);
            Invoke("isAttackedFalse", 0.5f);
            bossWay.GetComponent<BossScript>().ReturntoMove();
            return;
        }
        playerController.Move(movingWay * playerSpeed * Time.deltaTime);
        anim.SetBool("IsRun", movingWay != Vector3.zero);
    }

    void Gravity()
    {
        jumpVector = new Vector3(0f, -20.0f, 0f);
        playerController.Move(jumpVector * Time.deltaTime);
    }

    void Dodge()
    {
        if (jDown && movingWay != Vector3.zero && !isDodge)
        {
            playerSpeed *= 2;
            isDodge = true;
            
            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        playerSpeed *= (float)0.5;
        Invoke("DodgeFalse", 10.0f);
    }

    void DodgeFalse()
    {
        isDodge = false;
    }

    void Interaction()
    {
        if(!iDown) return;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2.5f))
        {
            if (hit.collider.CompareTag("Door"))
            {
                clickObject = hit.collider.gameObject;
                gameManager.Field_Change(clickObject);
                isLoading = true;
            }

            if (hit.collider.CompareTag("Test"))
            {
                Destroy(hit.transform.gameObject);
                for (int i = 0; i < 3; i++)
                {
                    GameObject.Find("Boss").transform.GetChild(i).gameObject.SetActive(true);
                    boss.GetComponent<BoxCollider>().enabled = true;
                    boss.GetComponent<BossScript>().enabled = true;
                    boss.GetComponent<EnemySkill>().enabled = true;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zone"))
        {
            playerSpeed *= (float)0.5;
            Debug.Log("?��?��?��?�� ?��?�� ?��?���?");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Zone"))
        {
            SlowOut();
        }
    }
    void SlowOut()
    {
        playerSpeed *= (float)2.0;
        Debug.Log("?��?��?��?�� ?��?�� ?��?��?��?��");
    }

    void isAttackedFalse()
    {
        isAttacked = false;
    }
    
    public void DamageAction(int damage)
    {
        hp -= damage;
        StartCoroutine(shpwredScreen());
        print("?��?�� ?��??? 체력: " + hp);
    }
    IEnumerator shpwredScreen()//���� ���? �� ó��
    {
        print("hit");
        redScreen.color = new Color(255, 0, 0, UnityEngine.Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);//0.5�� �Ŀ� ���¸� attack���� ����->0.5�� ���� Attacking ���°� �����Ǹ� �� ������ ���ʹ̰� ������ ����
        redScreen.color = Color.clear;
    }
}