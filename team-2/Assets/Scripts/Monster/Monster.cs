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
    // 다음 행동을 주기적으로 업데이트
    [SerializeField] float dt;
    [SerializeField] float nextBehaviourTime;
    [SerializeField] bool isLive;

    private void OnDrawGizmos()
    {
        // 플레이어 인식 및 공격 반경 범위를 기즈모로 그려준다. 붉은색은 추적 범위, 파란색은 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        detectCollider = GetComponentInChildren<CapsuleCollider>();
        Debug.Log("나는 가져온다 콜라이더를 " + detectCollider.gameObject.name);
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
            // 타겟이 추적 반경에 들어왔을 때
            if (distance <= detectRange)
            {   // 현재 상태가 Idle 정지 상태일때
                if (state == AIState.idle)
                {

                }

                // 현재 상태가 추적중일 때
                if (state == AIState.run)
                {   // Nav 컴포넌트를 통해 목적지를 타겟 포지션으로 설정한다.
                    agent.SetDestination(target.position);
                    anim.SetFloat("speed", agent.velocity.magnitude);

                    if (distance <= attackRange)
                    {   // 공격 거리 안으로 들어왔을 때

                    }
                }
            }
            else
            {
                // 현재 상태가 추적중일때 타겟이 추적범위 밖으로 벗어날 시
                if (state == AIState.run)
                {   // 처음 위치해있던 지점으로 돌아간다.
                    anim.SetFloat("speed", agent.velocity.magnitude);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("부딫힌 사람? " + other.gameObject.name);
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
