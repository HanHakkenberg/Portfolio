using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : Soldier {
    public List<Soldier> attackingSoldiers = new List<Soldier>();
    public int maxAttacking;
    public bool attackingCastle;
    public Damagebles target;

    [HideInInspector]
    public bool slowed;

    void Start() {
        MyStart();
        FindNewTarget();
        agent.SetDestination(targetTransform.position);
        BattleManager.instance.freeEnemys.Add(this);
    }

    void Update() {
        FindNewTarget();
    }

    public void RemoveCounter(Soldier _attacking) {
        StopCoroutine(Attack());
        attackingSoldiers.Remove(_attacking);
        if(attackingSoldiers.Count == maxAttacking - 1) {
            BattleManager.instance.freeEnemys.Add(this);
        }
        else if(attackingSoldiers.Count <= 0) {
            FindNewTarget();
            if(attackingCastle) {
                target = targetTransform.GetComponent<Damagebles>();
            }
            else {
                agent.isStopped = false;
            }
        }
        else {
            target = attackingSoldiers[0];
        }
    }

    public void AddCounter(Allie _attacking) {
        attackingSoldiers.Add(_attacking);
        if(attackingSoldiers.Count >= maxAttacking) {
            target = _attacking;
            BattleManager.instance.freeEnemys.Remove(this);
        }
    }

    public void StartBattle(Damagebles _target) {
        StopCoroutine(Attack());
        anim.SetBool("Attack", true);
        target = _target;
        agent.isStopped = true;
        StartCoroutine(Attack());
        if(_target != null) {
            transform.LookAt(targetTransform);
        }
    }

    public void StopBattle() {
        if(attackingCastle == true) {
            StartBattle(targetTransform.GetComponent<Damagebles>());
        }
        else {
            anim.SetBool("Attack", false);
            StopCoroutine(Attack());
            target = null;
            agent.isStopped = false;
            FindNewTarget();
        }
    }

    public virtual void FindNewTarget() {
        if(agent != null) {
            if(targetTransform != null) {
                if(targetTransform.tag == "Ally") {
                    agent.SetDestination(targetTransform.position);
                    return;
                }
                Transform newTarget = BattleManager.instance.EnemyGetTarget(transform.position.x);
                if(targetTransform != newTarget) {
                    anim.SetBool("Attack", false);
                    StopCoroutine(Attack());
                    target = null;
                    targetTransform = newTarget;
                    agent.SetDestination(targetTransform.position);
                    attackingCastle = false;
                    if(attackingSoldiers.Count <= 0) {
                        agent.isStopped = false;
                    }
                }
            }
            else {
                targetTransform = BattleManager.instance.EnemyGetTarget(transform.position.x);
                if(targetTransform == null) {
                    return;
                }
                agent.SetDestination(targetTransform.position);
                anim.SetBool("Attack", false);
                attackingCastle = false;
                StopCoroutine(Attack());
                if(attackingSoldiers.Count <= 0) {
                    agent.isStopped = false;
                }
            }
        }

    }

    public virtual IEnumerator Attack() {
        if(targetTransform.GetComponent<Damagebles>() != null) {
            Damagebles _targetToAttack = targetTransform.GetComponent<Damagebles>();
            yield return new WaitForSeconds(attackCooldown);
            if(target == null) {
                if(attackingSoldiers.Count > 0) {
                    for(int i = 0; i < attackingSoldiers.Count; i++) {
                        if(attackingSoldiers[i].inFight == true) {

                            target = attackingSoldiers[i];
                            break;
                        }
                    }
                }
                StopBattle();
            }
            else {
                StopAllCoroutines();
                StartCoroutine(Attack());
                target.TakeDamage(myStats.damage.currentValue);
            }
        }
    }

    private void OnDisable() {
        StopCoroutine(Attack());
        WaveManager.instance.RemoveEnemyFromScene(this);
    }

    private void OnEnable() {
        FindNewTarget();
        agent.SetDestination(targetTransform.position);
    }
}