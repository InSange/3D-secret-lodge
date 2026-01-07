using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 몬스터 타입
/// 나중에 플레이어가 사망한 몬스터의 종류에 따라
/// 죽는 애니메이션을 다르게 해주기 위해서 열거형으로 타입을 선언해주었다.
/// </summary>
public enum MonsterType
{
    Zombie = 0,
    Mutant,
    Crusader,
    Titan,
    Rocks,
    Axe,
    Lava
}
/// <summary>
/// 몬스터가 지니는 상태는 Idle(대기), Patrol(순찰), Chase(추격), Attack(공격)으로 구성되어 있다.
/// </summary>
public enum AIState
{
    idle,
    patrol,
    chase,
    attack
}
/// <summary>
/// AI 기능을 위해 몬스터 오브젝트들은 NavMeshAgent가 필요하다.
/// </summary>
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class Monster : MonoBehaviour
{   /// <summary>
    ///  몬스터가 지니고 있는 공통적인 변수들이다.
    /// </summary>
    // common component
    public CapsuleCollider detectCollider;  // 플레이어 감지를 위한 콜라이더
    public Transform target;    // 플레이어가 감지거리 안에 있으면 해당 변수에 플레이어 위치가 들어온다.
    public UnityEngine.AI.NavMeshAgent agent;   // AI기능을 위한 NavMeshAgent
    public Animator anim;   // state에 따른 애니메이션 조종하기 위한 애니메이터
    public MonsterType type;    // 몬스터별 타입
    // fixed status
    [SerializeField] float smoothRotationTime;  // Look at target rotation time
    [SerializeField] float rotationVelocity;    // Look at target rotation speed
    [SerializeField] int targetLayer;   // 콜라이더가 아닌 레이어로 탐지하는 방법

    // status
    public AIState state;   // 현재 몬스터가 실행중인 state상태 저장 변수
    [SerializeField] bool isChase;  // 추적중인가?
    [SerializeField] bool isLive;   // 살아있는가?
    [SerializeField] float dt;  // 다음 행동 및 분석을 위한 시간을 담아둘 dt변수
    public float detectRange;   // 감지 거리
    public float attackRange;   // 공격 거리
    public float nextBehaviourTime; // 다음 행동까지의 시간
    public float patrolDistance;    // 순찰 거리
    public float speed; // 기본 속도
    public float chaseSpeed;    // 추적 속도
    public Vector3 attackSize;  // 공격 범위
    public float attackHeight;  // 공격 높이(월드 포지션 값과 메쉬 값의 위치는 다르기 때문에 몬스터 높이 만큼 추가)
    /// <summary>
    /// 보호수준을 위해서 해당 변수들을 private으로 하였지만
    /// 하위 클래스 접근을 위해서 public으로 해도 상관없을 것 같다.
    /// </summary>
    /// <returns></returns>
    public ref float GetRotationTime()
    {
        return ref smoothRotationTime;
    }
    public float GetRotationVelocity()
    {
        return rotationVelocity;
    }
    /// <summary>
    /// 탐지 거리, 공격 거리를 시각적으로 확인할 수 있도록 나타냄
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + transform.forward * 1.0f + transform.up * attackHeight, attackSize);
    }
    /// <summary>
    /// 몬스터 오브젝트의 기초 세팅
    /// </summary>
    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        detectCollider = GetComponentInChildren<CapsuleCollider>();
        anim = GetComponent<Animator>();

        // fix status.
        smoothRotationTime = 0.3f;
        rotationVelocity = 0.1f;
        isLive = true;
        dt = 0;
        targetLayer = 1 << LayerMask.NameToLayer("Player");
        // mosnter setting for monster stats
        MonsterSetting();
        detectCollider.radius = detectRange;
    }

    /// <summary>
    /// 몬스터가 살아 있다면
    /// 다음 행동 시간이 되면 그 다음 행동을 위해 행동 업데이트를 진행한다.
    /// </summary>
    void Update()
    {   // if monster state live check update for next behaviour
        if(isLive)
        {
            dt += Time.deltaTime;
            if(dt >= nextBehaviourTime)
            {
                MonsterAI();
                dt = 0;
            }

            anim.SetFloat("speed", agent.desiredVelocity.magnitude);
        }
    }
    /// <summary>
    /// 변경이 가능한 몬스터 세팅 수치
    /// 기본적인 수치 값이 있고 해당 값을 상속 받는 하위 클래스에서 수정이 가능하다.
    /// </summary>
    public virtual void MonsterSetting()
    {   // setting Init Monster status
        detectRange = 10.0f;
        attackRange = 3.0f;
        nextBehaviourTime = 0.5f;
        patrolDistance = 10.0f;
        speed = 1.0f;
        chaseSpeed = 5.0f;
        attackSize = new Vector3(2.0f, 4.0f, 2.0f);
        attackHeight = 2.0f;
    }
    /// <summary>
    /// 현재 state에 따른 행동 업데이트
    /// 타겟이 있느냐 없느냐에 따라 다음 행동(state)들이 결정이 된다.
    /// </summary>
    public virtual void MonsterAI()
    {
        if (target == null)
        {
            // if target null you continue patrol state
            if (state != AIState.patrol)
            {
                state = AIState.patrol;
                anim.SetBool("chase", false);
                agent.speed = speed;
            }
            // Patrol for detect Player if less than 1.0f to destination you can check next patrol site(position)
            if (agent.remainingDistance <= 1.0f)
            {   // 타겟이 없고 목적지와 거리가 1.0보다 작거나 같을 경우 탐색거리 내에서 랜덤한 포지션을 불러온다.
                var randomPos = Random.insideUnitSphere * patrolDistance + transform.position; 

                UnityEngine.AI.NavMeshHit hit;
                
                UnityEngine.AI.NavMesh.SamplePosition(randomPos, out hit, patrolDistance, UnityEngine.AI.NavMesh.AllAreas); 
                agent.SetDestination(hit.position);
            }
        }
        else//if(target != null)
        {   // continue chase state
            if (state != AIState.chase)
            {
                state = AIState.chase;
                anim.SetBool("chase", true);
                agent.speed = chaseSpeed;
            }
            // rotate to target 몬스터 오브젝트가 타겟을 향해 부드럽게 쳐다보도록 회전하는 기능들
            var lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;
            // look at target position
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref rotationVelocity, smoothRotationTime);
            // target distances
            float dist = Vector3.Distance(target.position, transform.position);
            // 타겟과의 현재 거리에 따라서 공격을 할지, 플레이어 위치로 추적을 할지 결정된다.
            if (dist <= attackRange)
            {   // if Player in attack Range start attack anim
                anim.SetTrigger("attack");
            }
            else
            {
                agent.SetDestination(target.position);
            }
        }
    }
    /// <summary>
    /// 몬스터의 공격 기능
    /// 공격 범위는 AttackSize의 크기만큼 오브젝트의 바로 앞에서 실행된다.
    /// 해당 범위안에 탐지된 콜라이더를 가진 객체들은 col에 저장이 되며 우리는 플레이어 태그를 가진
    /// 오브젝트를 찾아서 게임 오버 이벤트를 실행시켜주었다.
    /// </summary>
    public virtual void MonsterAttack()
    {
        var col = Physics.OverlapBox(transform.position + transform.forward * 1.0f + transform.up * attackHeight, attackSize * 0.5f, transform.rotation);
        for(int i = 0; i < col.Length; i++)
        {
            if (col[i].CompareTag("Player") == false) continue;
            Player player = col[i].GetComponent<Player>();
            if (player.live)
            {
                player.live = false;
                GameManager.Instance.GameOver(type);
            }
        }
    }

    /// <summary>
    /// 콜라이더로 감지할 필요가 없이 Physics에 내장된 기능으로 감지할 수 있다.
    /// 구체모양의 감지가 해당 함수가 호출될 때마다 실행된다.
    /// </summary>
    /// <param name="other"></param>
    /*void SearchTarget()
    {
        var colliders = Physics.OverlapSphere(transform.position, detectRange, targetLayer);

        foreach (var collider in colliders)
        {
            var livingEntity = collider.GetComponent<Player>();

            if (livingEntity != null)
            {
                target = livingEntity.gameObject.transform;

                break;
            }
        }
    }*/
    
   // 감지 콜라이더 안으로 플레이어가 들어오면 타겟으로 설정. 나가면 NULL로 설정.
    private void OnTriggerEnter(Collider other)
    {
        // Check Player in detect Range
        if(other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject.transform;
            anim.SetBool("chase", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {   // Player out detect Range
        if(other.gameObject.CompareTag("Player"))
        {
            target = null;
        }
    }
}
