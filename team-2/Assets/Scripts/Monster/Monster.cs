using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public enum AIState
{
    idle,
    walk,
    patrol,
    chase,
    attack
}

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class Monster : MonoBehaviour
{
    // common component
    public CapsuleCollider detectCollider;
    public Transform target;
    public UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
    public MonsterType type;
    // fixed status
    [SerializeField] float smoothRotationTime;// Look at target rotation time
    [SerializeField] float rotationVelocity;// Look at target rotation speed
    [SerializeField] int targetLayer;

    // status
    public AIState state;
    [SerializeField] bool isChase;
    [SerializeField] bool isLive;
    [SerializeField] float dt;
    public float detectRange;
    public float attackRange;
    public float nextBehaviourTime;
    public float patrolDistance;
    public float speed;
    public float chaseSpeed;
    public Vector3 attackSize;
    public float attackHeight;

    public ref float GetRotationTime()
    {
        return ref smoothRotationTime;
    }

    public float GetRotationVelocity()
    {
        return rotationVelocity;
    }

    public void SetDetectRange(float v)
    {
        detectRange = v;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + transform.forward * 1.0f + transform.up * attackHeight, attackSize);
    }

    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        detectCollider = GetComponentInChildren<CapsuleCollider>();
        anim = GetComponent<Animator>();
        Debug.Log("���� �����´� �ݶ��̴��� " + detectCollider.gameObject.name);
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

    // Update is called once per frame
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
            {
                var randomPos = Random.insideUnitSphere * patrolDistance + transform.position;  // center�� �������� �Ͽ� ������(�ݰ�) distance ���� ������ ��ġ ����. *Random.insideUnitSphere*�� ������ 1 ¥���� �� ������ ������ ��ġ�� �������ִ� ������Ƽ��.

                UnityEngine.AI.NavMeshHit hit;  // NavMesh ���ø��� ����� ���� �����̳�. Raycast hit �� ���

                UnityEngine.AI.NavMesh.SamplePosition(randomPos, out hit, patrolDistance, UnityEngine.AI.NavMesh.AllAreas);  // areaMask�� �ش��ϴ� NavMesh �߿��� randomPos�κ��� distance �ݰ� ������ randomPos�� *���� �����* ��ġ�� �ϳ� ã�Ƽ� �� ����� hit�� ����. 
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
            // rotate to target
            var lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;
            // look at target position
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref rotationVelocity, smoothRotationTime);
            // target distances
            float dist = Vector3.Distance(target.position, transform.position);
            
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

    public virtual void MonsterAttack()
    {
        Debug.Log("monster Attack");
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

    // If dont use OnTrigger you can use this for detect player 
    /*void SearchTarget()
    {
        var colliders = Physics.OverlapSphere(transform.position, detectRange, targetLayer);

        foreach (var collider in colliders)
        {
            var livingEntity = collider.GetComponent<Player>();

            // LivingEntity ������Ʈ�� �����ϸ�, �ش� LivingEntity�� ����ִٸ�,
            if (livingEntity != null)
            {
                // ���� ����� �ش� LivingEntity�� ����
                target = livingEntity.gameObject.transform;

                // for�� ���� ��� ����
                break;
            }
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        // Check Player in detect Range
        if(other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject.transform;
            anim.SetBool("chase", true);
            Debug.Log("추적한다 나는 " + target.name);
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
