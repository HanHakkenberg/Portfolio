using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlowZone : EffectZone
{

    private Dictionary<Enemy, float> slowedEnemies = new Dictionary<Enemy, float>();

    public float mySlowAmount;

    public override void Update()
    {
        base.Update();

        if (statsPopupPanel.activeInHierarchy)
        {
            statsText.text = "Slow: <color=green>" + ((1 - mySlowAmount) * 100) + "%</color> Health: <color=green>" + myHealth + "</color>/<color=green>" + myMaxHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canEffect)
        {
            return;
        }

        if (myHealth <= 0)
        {
            return;
        }

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (enemy.slowed)
            {
                return;
            }

            slowedEnemies.Add(enemy, enemy.agent.speed);
            enemy.agent.speed *= mySlowAmount;
            enemy.slowed = true;
            myHealth--;

            //if (anim != null)
            //{
            //    anim.SetTrigger("Damage");
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (slowedEnemies.ContainsKey(enemy))
            {
                float speed = slowedEnemies[enemy];
                enemy.agent.speed = speed;
                enemy.slowed = false;

                slowedEnemies.Remove(enemy);
            }
        }

        if (myHealth <= 0)
        {
            ObjectPooler.instance.GrabFromPool("demolish particle", transform.position, Quaternion.identity);
            ObjectPooler.instance.AddToPool(myObjectPool, gameObject);
        }
    }
}
