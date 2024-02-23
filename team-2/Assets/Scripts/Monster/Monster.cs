using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // ������Ʈ��
    public CapsuleCollider detectCollider;
    public Transform target;
    public UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
    // ���� ����
    [SerializeField] float smoothRotationTime;//target ������ ȸ���ϴµ� �ɸ��� �ð�
    [SerializeField] float rotationVelocity;//target �ӵ��� �ٲ�µ� �ɸ��� �ð�
    [SerializeField] int targetLayer;

    // ���°���
    public AIState state;
    [SerializeField] bool isChase;
    [SerializeField] bool isLive;
    [SerializeField] float dt;
    public float detectRange;
    public float attackRange;
    public float nextBehaviourTime;
    public float patrolDistance;

    public ref float GetRotationTime()
    {
        return ref smoothRotationTime;
    }

    public float GetRotationVelocity()
    {
        return rotationVelocity;
    }


    private void OnDrawGizmos()
    {
        // �÷��̾� �ν� �� ���� �ݰ� ������ ������ �׷��ش�. �������� ���� ����, �Ķ����� ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        detectCollider = GetComponentInChildren<CapsuleCollider>();
        anim = GetComponent<Animator>();
        Debug.Log("���� �����´� �ݶ��̴��� " + detectCollider.gameObject.name);
        // �⺻ ����
        smoothRotationTime = 0.3f;
        rotationVelocity = 0.1f;
        isLive = true;
        dt = 0;
        targetLayer = LayerMask.NameToLayer("Player");
        // ���� ����
        detectRange = 10.0f;
        attackRange = 3.0f;
        nextBehaviourTime = 0.5f;
        patrolDistance = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
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

    public virtual void MonsterAI()
    {
        if (target == null)
        {
            Debug.Log("Ÿ �� �� ��");

            if (state != AIState.patrol)
            {
                state = AIState.patrol;
                anim.SetBool("chase", false);
                agent.speed = 1.0f;
                Debug.Log("Ž����");
            }

            if (agent.remainingDistance <= 1.0f)
            {
                var randomPos = Random.insideUnitSphere * patrolDistance + transform.position;  // center�� �������� �Ͽ� ������(�ݰ�) distance ���� ������ ��ġ ����. *Random.insideUnitSphere*�� ������ 1 ¥���� �� ������ ������ ��ġ�� �������ִ� ������Ƽ��.

                UnityEngine.AI.NavMeshHit hit;  // NavMesh ���ø��� ����� ���� �����̳�. Raycast hit �� ���

                UnityEngine.AI.NavMesh.SamplePosition(randomPos, out hit, patrolDistance, UnityEngine.AI.NavMesh.AllAreas);  // areaMask�� �ش��ϴ� NavMesh �߿��� randomPos�κ��� distance �ݰ� ������ randomPos�� *���� �����* ��ġ�� �ϳ� ã�Ƽ� �� ����� hit�� ����. 
                agent.SetDestination(hit.position);
            }
        }
        else//if(target != null)
        {
            if (state != AIState.chase)
            {
                state = AIState.chase;
                anim.SetBool("chase", true);
                agent.speed = 5.0f;
            }

            var lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref rotationVelocity, smoothRotationTime);

            float dist = Vector3.Distance(target.position, transform.position);
            // Ÿ���� ���� �ݰ濡 ������ ��
            if (dist <= attackRange)
            {   // ���� ���°� Idle ���� �����϶�
                anim.SetTrigger("attack");
            }
            else
            {
                agent.SetDestination(target.position);
            }
        }
    }

    void SearchTarget()
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
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("�΋H�� ���? " + other.gameObject.name);
        if(other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject.transform;
            anim.SetBool("chase", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            target = null;
        }
    }
}
