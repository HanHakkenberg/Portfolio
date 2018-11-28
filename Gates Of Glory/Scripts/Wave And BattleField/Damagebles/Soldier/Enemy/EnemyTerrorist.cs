using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTerrorist : Enemy {

    void Start() {
        MyStart();
        FindNewTarget();
        agent.SetDestination(targetTransform.position);
    }

    public override void TakeDamage(float damage) {
        myStats.health.currentValue -= damage;

        if(myStats.health.currentValue <= 0) {
            ObjectPooler.instance.AddToPool("Enemy Terrorist", gameObject);
            ResourceManager.instance.AddGold(ResourceManager.instance.terroristGoldReward);
        }
    }

    public override void MyStart() {
        agent.speed = Random.Range(1.75f, 2.25f);
        transform.localScale *= Random.Range(0.9f, 1.1f);
    }

    void OnCollisionEnter(Collision collision) {
        if(targetTransform != null && collision.transform == targetTransform) {
            collision.gameObject.GetComponent<CastleDeffensePoint>().DirectDamage(myStats.damage.currentValue);
            ObjectPooler.instance.GrabFromPool("terrorist kaboom", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
            ObjectPooler.instance.AddToPool("Enemy Terrorist", gameObject);
        }
    }
}
