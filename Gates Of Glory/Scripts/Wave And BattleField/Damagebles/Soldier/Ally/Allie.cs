using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Allie : Soldier {
    void Start() {
        targetTransform = BattleManager.instance.AllyGetTarget(transform.position.x, this, targetTransform);
        if(agent != null) {
            agent.SetDestination(targetTransform.position);
        }
    }

    void Update() {
        if(targetTransform.tag == "Enemy" && agent.isStopped == false) {
            agent.SetDestination(targetTransform.position);
            anim.SetBool("Idle", false);
        }
        GetNewTarget();
    }

    public void GetNewTarget() {
        if(targetTransform != null && targetTransform.gameObject.activeInHierarchy == false) {
            targetTransform = null;
            inFight = false;
            anim.SetBool("Attack", false);
        }
        if(inFight) {
            agent.isStopped = true;
            anim.SetBool("Attack", true);
            anim.SetBool("Idle", false);
            return;
        }
        if(targetTransform != null) {
            Transform newTarget = BattleManager.instance.AllyGetTarget(transform.position.x, this, targetTransform);
            if(newTarget != targetTransform) {
                if(targetTransform.tag == "Enemy") {
                    agent.isStopped = false;
                    if(newTarget.tag == "Enemy") {
                        targetTransform.GetComponent<Enemy>().RemoveCounter(this);
                        newTarget.GetComponent<Enemy>().AddCounter(this);
                        targetTransform = newTarget;
                        agent.SetDestination(targetTransform.position);
                        agent.isStopped = false;
                    }
                    else if(agent.isStopped == false) {
                        agent.SetDestination(targetTransform.position);
                    }
                    anim.SetBool("Idle", false);
                }
                else {
                    if(newTarget.tag == "Enemy") {
                        newTarget.GetComponent<Enemy>().AddCounter(this);
                    }
                    targetTransform = newTarget;
                    agent.SetDestination(targetTransform.position);
                    agent.isStopped = false;
                    anim.SetBool("Idle", false);
                }
            }
        }
        else {
            Transform newTarget = BattleManager.instance.AllyGetTarget(transform.position.x, this, targetTransform);
            if(newTarget.tag == "Enemy") {
                newTarget.GetComponent<Enemy>().AddCounter(this);
            }
            targetTransform = newTarget;
            agent.SetDestination(targetTransform.position);
            agent.isStopped = false;
            anim.SetBool("Idle", false);
        }
    }

    public override void TakeDamage(float damage) {
        myStats.health.currentValue -= damage;
        if(myStats.health.currentValue <= 0) {
            if(targetTransform != null) {
                targetTransform.GetComponent<Enemy>().RemoveCounter(this);
            }
            Destroy(gameObject);
        }
    }

    public virtual IEnumerator Attack() {
        Transform _attackingCurrently = targetTransform;
        yield return new WaitForSeconds(attackCooldown);
        if(targetTransform != null && targetTransform.tag == "Enemy" && targetTransform == _attackingCurrently) {
            targetTransform.GetComponent<Enemy>().TakeDamage(myStats.damage.currentValue);
            StartCoroutine(Attack());
        }
        else {
            StopCoroutine(Attack());
            anim.SetBool("Attack", false);
            anim.SetBool("Idle", false);
        }
    }

    private void OnDisable() {
        WaveManager.instance.alliesInScene.Remove(this);
    }
}
