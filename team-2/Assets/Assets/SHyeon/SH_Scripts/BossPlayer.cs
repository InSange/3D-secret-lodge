using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.AI;
//using UnityEngine.UIElements;

public class BossPlayer : MonoBehaviour
{
    private GameObject ending;
    public enum playState//ï¿½Ã·ï¿½ï¿½Ì¾ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
    {
        Normal,
        Stun
    }
    public float hp = 15f;
    public int attackPower = 3;//?”Œ? ˆ?´?–´?˜ ê³µê²©? ¥
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
    public Transform attackPoint; //?”Œ? ˆ?´?–´?˜ ê³µê²©?´ ?‚˜ê°??Š” ?œ„ì¹?

    
    
    public bool isLoading;  // ï¿½Îµï¿½ï¿½ï¿½ï¿½Ï¶ï¿½ ï¿½Ã·ï¿½ï¿½Ì¾ï¿½ ï¿½Ï½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿?(ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ x).
    
    GameObject clickObject;  // ï¿½Ã·ï¿½ï¿½Ì¾î°¡ ï¿½ï¿½È£ï¿½Û¿ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Æ®ï¿½ï¿½ ï¿½Ö¾ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½.

    public GameManager gameManager; // ï¿½ï¿½ï¿½Ó¸Å´ï¿½ï¿½ï¿½
    public playState p_State;
    public Transform enemypos;
    float currentTime;// ê³µê²© ?”œ? ˆ?´?š© ë³??ˆ˜
    float attackDelay = 0.5f;// ê³µê²© ê°? ?”œ? ˆ?´ê°?
    
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
        p_State = playState.Normal;//ï¿½Ã·ï¿½ï¿½Ì¾ï¿½ ï¿½ï¿½ï¿½Â¸ï¿½ ï¿½âº» ï¿½ï¿½ï¿½Â·ï¿½ 
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(p_State == playState.Stun)//?Š¤?„´(?  ?œ„ì¹˜ë¡œ ?”Œ? ˆ?´?–´ ?Œ?–´?‹¹ê¸°ê¸°) ?ƒ?ƒœê°? ?˜ë©? ?‹¤?–‰
        {
            Vector3 way = (enemypos.position - transform.position).normalized;//?”Œ? ˆ?´?–´ë¥? ?‹¹ê²¨ì˜¤ê¸? ?œ„?•´ ë²¡í„°ë¡? ë°©í–¥?„ ê³„ì‚°?•˜ê³?
            transform.Translate(way * 2 * Time.deltaTime, Space.World);//?Š¤?„´ ?™?•ˆ ?•´?‹¹ ê°’ë§Œ?¼ ?‹¹ê²¨ì˜¨?‹¤.
            return;//?”Œ? ˆ?´?–´ê°? ?Š¤?„´ ?ƒ?ƒœë©? ?œ„?˜ ?‚´?š©ë¨? ?‹¤?–‰?‹œ?‚¤ê³? ë¦¬í„´->?‚¬?š©??˜ ???ì§ì„ ?…? ¥?„ ë°›ì??ëª»í•¨
        }
        Attack();//ê³µê²© ì²˜ë¦¬ ?•¨?ˆ˜
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

    private void Attack()///ê³µê²© ?ˆ˜?–‰ ?•¨?ˆ˜
    {
        currentTime += Time.deltaTime;//ê³µê²© ì¿¨í???„ 
        if (currentTime > attackDelay)//ê³µê²© ì¿¨í???„?´ ì°¨ë©´ ê³µê²© ê°??Š¥
        {
            if (Input.GetMouseButtonDown(0))//ê³µê²© ì¿¨í???„?´ ì°? ?ƒ?ƒœ?—?„œ ë§ˆìš°?Š¤ ì¢Œí´ë¦? ?‹œ ê³µê²©
            {
                print("ê³µê²© ?‹œ? „");
                Ray ray = new Ray(attackPoint.position, attackPoint.forward);
                Debug.DrawRay(ray.origin, ray.direction * 3f, Color.blue);
                RaycastHit hit;
                currentTime = 0;//ê³µê²© ì¿¨í???„ ?Œë¦¬ê¸°
                if (Physics.Raycast(ray, out hit, 3f)) //1,5.ê¸¸ì´?˜ ì§§ì?? ? ˆ?´(ê·¼ì ‘ ê³µê²©)?— ë³´ìŠ¤ê°? ë§ìœ¼ë©? ë³´ìŠ¤?˜ ?°ë¯¸ì?? ì²˜ë¦¬ ?•¨?ˆ˜ë¥? ë¶ˆëŸ¬??? ê³µê²© 
                {
                    if (hit.collider.CompareTag("Test"))///////////////////////////////ë³´ìŠ¤ ?ƒœê·¸ì´ë¦?
                    {
                        bossWay.GetComponent<BossScript>().HitEnemy(attackPower);
                        print("ë³´ìŠ¤ ê³µê²©");
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
            Debug.Log("?”Œ? ˆ?´?–´ ?†?„ ?Š? ¤ì§?");
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
        Debug.Log("?”Œ? ˆ?´?–´ ?†?„ ?˜?Œ?•„?˜´");
    }

    void isAttackedFalse()
    {
        isAttacked = false;
    }
    
    public void DamageAction(int damage)
    {
        hp -= damage;
        StartCoroutine(shpwredScreen());
        print("?˜„?¬ ?‚¨??? ì²´ë ¥: " + hp);
    }
    IEnumerator shpwredScreen()//ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿? ï¿½ï¿½ Ã³ï¿½ï¿½
    {
        print("hit");
        redScreen.color = new Color(255, 0, 0, UnityEngine.Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);//0.5ï¿½ï¿½ ï¿½Ä¿ï¿½ ï¿½ï¿½ï¿½Â¸ï¿½ attackï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½->0.5ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ Attacking ï¿½ï¿½ï¿½Â°ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ç¸ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½Ê¹Ì°ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        redScreen.color = Color.clear;
    }
}