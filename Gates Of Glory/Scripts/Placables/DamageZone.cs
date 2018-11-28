using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageZone : EffectZone
{

    public int myDamage;

    public override void Update()
    {
        base.Update();

        if (statsPopupPanel.activeInHierarchy)
        {
            statsText.text = "Damage: <color=green>" + myDamage + "</color> Health: <color=green>" + myHealth + "</color>/<color=green>" + myMaxHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canEffect)
        {
            return;
        }

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(myDamage);
            myHealth--;

            if (anim != null)
            {
                anim.SetTrigger("Damage");
            }
        }

        if (myHealth <= 0)
        {
            ObjectPooler.instance.GrabFromPool("demolish particle", transform.position, Quaternion.identity);
            ObjectPooler.instance.AddToPool(myObjectPool, gameObject);
        }
    }
}
