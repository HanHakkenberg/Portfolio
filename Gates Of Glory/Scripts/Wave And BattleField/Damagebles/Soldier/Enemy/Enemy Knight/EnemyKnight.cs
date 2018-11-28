using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnight : Enemy {

    public override void TakeDamage(float damage) {
        myStats.health.currentValue -= damage;

        if(myStats.health.currentValue <= 0) {
            StopAllCoroutines();
            ObjectPooler.instance.AddToPool("Enemy Knight", gameObject);
            ResourceManager.instance.AddGold(ResourceManager.instance.normalEnemyGoldReward);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.transform == targetTransform) {
            StartBattle(target);
            if(targetTransform.tag == "Defense") {
                targetTransform.GetComponent<CastleDeffensePoint>().attackingMe.Add(this);
            }
            agent.isStopped = true;
            attackingCastle = true;
            target = collision.gameObject.GetComponent<Damagebles>();
        }
    }

    void OnTriggerStay(Collider other) {
        if(Vector3.Distance(transform.position,other.transform.position) < 1 && other.tag == "Ally" && other.GetComponent<Allie>().targetTransform == gameObject.transform) {
            StartBattle(other.GetComponent<Allie>());
        }
    }

    void OnTriggerExit(Collider other) {
        if(targetTransform != null && targetTransform.gameObject.activeSelf == false) {
            targetTransform = null;
            StopBattle();
            FindNewTarget();
        }
    }
}
