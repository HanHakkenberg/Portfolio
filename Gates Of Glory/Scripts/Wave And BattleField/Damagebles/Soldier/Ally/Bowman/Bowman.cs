
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowman : Allie {
    public Transform bowPos;
    public float fireRate;

    void OnTriggerEnter(Collider other) {
        if(targetTransform != null && targetTransform == other.transform) {
            if(other.GetComponent<Enemy>() is EnemyBowman) {
                targetTransform.gameObject.GetComponent<Enemy>().StartBattle(this);
            }
            else {
                other.GetComponent<Enemy>().targetTransform = gameObject.transform;
            }
            anim.SetBool("Attack", true);
            anim.SetBool("Idle", false);
            inFight = true;
            agent.isStopped = true;
            StopCoroutine(Attack());
            StartCoroutine(Attack());
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.transform == targetTransform) {
            print("l");
            agent.isStopped = true;
            if(transform.position.x > 0) {
                transform.localEulerAngles = new Vector3(0, 90, 0);
            }
            else {
                transform.localEulerAngles = new Vector3(0, -90, 0);
            }
            anim.SetBool("Idle", true);
        }
    }

    void OnCollisionStay(Collision collision) {
        if(collision.transform != targetTransform) {
            GetNewTarget();
        }
        else {
            anim.SetBool("Idle", true);
            agent.isStopped = true;
            GetNewTarget();
        }
    }

    public override IEnumerator Attack() {
        Transform _attackingCurrently = targetTransform;
        yield return new WaitForSeconds(attackCooldown);

        if(targetTransform != null && targetTransform.tag == "Enemy" && targetTransform == _attackingCurrently) {
            targetTransform.GetComponent<Enemy>().TakeDamage(myStats.damage.currentValue);
            StartCoroutine(Attack());
            float distance = Vector3.Distance(bowPos.position, targetTransform.position);
            Transform _currentArrow = ObjectPooler.instance.GrabFromPool("Ally Arrows", bowPos.position, Quaternion.Euler(new Vector3(0, 0, 0))).transform;
            _currentArrow.LookAt(targetTransform);
            _currentArrow.GetChild(0).GetComponent<Arrow>().distance = distance;
            _currentArrow.position += _currentArrow.forward * distance / 2;
            _currentArrow.GetChild(0).GetComponent<Arrow>().myArrow.position -= new Vector3(0, _currentArrow.GetChild(0).GetComponent<Arrow>().minAmount, 0);
            _currentArrow.GetChild(0).transform.position = bowPos.position;
            _currentArrow.GetChild(0).transform.rotation = bowPos.rotation;
        }
        else {
            StopCoroutine(Attack());
            anim.SetBool("Attack", false);
            anim.SetBool("Idle", false);
        }

    }

    public override void TakeDamage(float damage) {
        myStats.health.currentValue -= damage;

        if(myStats.health.currentValue <= 0) {
            StopAllCoroutines();
            if(targetTransform != null) {
                targetTransform.GetComponent<Enemy>().RemoveCounter(this);
            }
            ObjectPooler.instance.AddToPool("Ally Bowman", gameObject);
            ResourceManager.instance.AddGold(ResourceManager.instance.normalEnemyGoldReward);
        }
    }
}
