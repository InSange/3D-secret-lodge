using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolZombie : Monster
{
    public override void MonsterSetting()
    {
        base.MonsterSetting();
        detectRange = 5.0f;
        attackRange = 3.0f;
        nextBehaviourTime = 0.5f;
        patrolDistance = 10.0f;
        speed = 1.0f;
        chaseSpeed = 5.0f;
        type = MonsterType.Zombie;
    }
    public override void MonsterAI()
    {
        if (target == null)
        {
            Debug.Log("타 겟 없 음");

            if (state != AIState.patrol)
            {
                state = AIState.patrol;
                anim.SetBool("chase", false);
                agent.speed = speed;
                Debug.Log("탐색중");
            }

            if (agent.remainingDistance <= 1.0f)
            {
                var randomPos = Random.insideUnitSphere * patrolDistance + transform.position;  // center를 중점으로 하여 반지름(반경) distance 내에 랜덤한 위치 리턴. *Random.insideUnitSphere*은 반지름 1 짜리의 구 내에서 랜덤한 위치를 리턴해주는 프로퍼티다.

                UnityEngine.AI.NavMeshHit hit;  // NavMesh 샘플링의 결과를 담을 컨테이너. Raycast hit 과 비슷

                UnityEngine.AI.NavMesh.SamplePosition(randomPos, out hit, patrolDistance, UnityEngine.AI.NavMesh.AllAreas);  // areaMask에 해당하는 NavMesh 중에서 randomPos로부터 distance 반경 내에서 randomPos에 *가장 가까운* 위치를 하나 찾아서 그 결과를 hit에 담음. 
                agent.SetDestination(hit.position);
            }
        }
        else//if(target != null)
        {
            if (state != AIState.chase)
            {
                state = AIState.chase;
                anim.SetBool("chase", true);
                agent.speed = chaseSpeed;
            }

            var lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref GetRotationTime() , GetRotationVelocity());

            float dist = Vector3.Distance(target.position, transform.position);
            // 타겟이 추적 반경에 들어왔을 때
            if (dist <= attackRange)
            {   // 현재 상태가 Idle 정지 상태일때
                anim.SetTrigger("attack");
                transform.LookAt(target);
                MonsterAttack();
            }
            else
            {
                agent.SetDestination(target.position);
            }
        }
    }
}
