using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Allie
{

    void OnTriggerEnter(Collider other)
    {
        if(targetTransform != null && targetTransform == other.transform) {
            targetTransform.gameObject.GetComponent<Enemy>().StartBattle(this);
            anim.SetBool("Attack", true);
            anim.SetBool("Idle", false);
            inFight = true;
            agent.isStopped = true;
            StopAllCoroutines();
            StartCoroutine(Attack());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == targetTransform)
        {
            agent.isStopped = true;
            if(transform.position.x > 0) {
                transform.localEulerAngles = new Vector3(0,90,0);
            }
            else {
                transform.localEulerAngles = new Vector3(0, -90, 0);
            }
            anim.SetBool("Idle", true);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.transform != targetTransform)
        {
            GetNewTarget();
        }
        else
        {
            anim.SetBool("Idle", true);
            GetNewTarget();
        }
    }

    public override void TakeDamage(float damage) {
        myStats.health.currentValue -= damage;

        if(myStats.health.currentValue <= 0) {
            StopAllCoroutines();
            if(targetTransform != null) {
                targetTransform.GetComponent<Enemy>().RemoveCounter(this);
            }
            ObjectPooler.instance.AddToPool("Ally Knight", gameObject);
            ResourceManager.instance.AddGold(ResourceManager.instance.normalEnemyGoldReward);
        }
    }
}
