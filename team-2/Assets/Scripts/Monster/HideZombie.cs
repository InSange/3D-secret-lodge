using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideZombie : Monster
{
    public override void MonsterAI()
    {
        if (target == null)
        {
            Debug.Log("Ÿ �� �� ��");

            if (state != AIState.idle)
            {
                state = AIState.patrol;
                anim.SetBool("chase", false);
                agent.speed = 1.0f;
                Debug.Log("Ž����");
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

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref GetRotationTime(), GetRotationVelocity());

            float dist = Vector3.Distance(target.position, transform.position);
            // Ÿ���� ���� �ݰ濡 ������ ��
            if (dist <= attackRange)
            {   // ���� ���°� Idle ���� �����϶�
                anim.SetTrigger("attack");
                transform.LookAt(target);
            }
            else
            {
                agent.SetDestination(target.position);
            }
        }
    }
}
