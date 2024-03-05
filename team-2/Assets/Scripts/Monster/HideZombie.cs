using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideZombie : Monster
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

            if (state != AIState.idle)
            {
                state = AIState.patrol;
                anim.SetBool("chase", false);
                agent.speed = speed;
                Debug.Log("탐색중");
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

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref GetRotationTime(), GetRotationVelocity());

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
