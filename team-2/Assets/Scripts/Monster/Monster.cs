using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    idle,
    walk,
    run,
    attack
}

[RequireComponent(typeof(NavMeshAgent))]

public class Monster : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float detectRange;
    [SerializeField] float attackRange;
    [SerializeField] CapsuleCollider detectCollider;
    [SerializeField] Transform target;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] AIState state;
    [SerializeField] bool isChase;
    [SerializeField] float distance;
    // ���� �ൿ�� �ֱ������� ������Ʈ
    [SerializeField] float dt;
    [SerializeField] float nextBehaviourTime;
    [SerializeField] bool isLive;

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
        agent = GetComponent<NavMeshAgent>();
        detectCollider = GetComponentInChildren<CapsuleCollider>();
        Debug.Log("���� �����´� �ݶ��̴��� " + detectCollider.gameObject.name);
        isLive = true;
        detectRange = 10.0f;
        attackRange = 5.0f;
        dt = 0;
        nextBehaviourTime = 1.0f;
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
        }
    }

    void MonsterAI()
    {
        if (target == null)
        {

        }
        else
        {
            distance = Vector3.Distance(target.position, transform.position);
            // Ÿ���� ���� �ݰ濡 ������ ��
            if (distance <= detectRange)
            {   // ���� ���°� Idle ���� �����϶�
                if (state == AIState.idle)
                {

                }

                // ���� ���°� �������� ��
                if (state == AIState.run)
                {   // Nav ������Ʈ�� ���� �������� Ÿ�� ���������� �����Ѵ�.
                    agent.SetDestination(target.position);
                    anim.SetFloat("speed", agent.velocity.magnitude);

                    if (distance <= attackRange)
                    {   // ���� �Ÿ� ������ ������ ��

                    }
                }
            }
            else
            {
                // ���� ���°� �������϶� Ÿ���� �������� ������ ��� ��
                if (state == AIState.run)
                {   // ó�� ��ġ���ִ� �������� ���ư���.
                    anim.SetFloat("speed", agent.velocity.magnitude);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�΋H�� ���? " + other.gameObject.name);
        if(other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject.transform;
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
