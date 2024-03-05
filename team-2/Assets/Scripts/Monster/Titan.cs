using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Titan : Monster
{
    public override void MonsterSetting()
    {
        base.MonsterSetting();
        detectRange = 10.0f;
        attackRange = 3.0f;
        nextBehaviourTime = 0.5f;
        patrolDistance = 10.0f;
        speed = 3.0f;
        chaseSpeed = 5.0f;
        type = MonsterType.Titan;
    }
    public override void MonsterAI()
    {
        if (target == null)
        {
            Debug.Log("Ÿ �� �� ��");

            if (state != AIState.patrol)
            {
                state = AIState.patrol;
                anim.SetBool("chase", false);
                agent.speed = speed;
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
                agent.speed = chaseSpeed;
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
