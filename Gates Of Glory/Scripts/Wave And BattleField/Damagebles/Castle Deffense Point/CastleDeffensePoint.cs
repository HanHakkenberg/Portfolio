using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleDeffensePoint : Damagebles {
    public List<Enemy> attackingMe = new List<Enemy>();
    public CastleGate myGate;
    public Image healthbarFill;

    public virtual void DirectDamage(float damage) {
        TakeDamage(damage);
    }

    public override void TakeDamage(float damage){
        myStats.health.currentValue -= damage;

        if (healthbarFill != null)
        {
            healthbarFill.fillAmount = (myStats.health.currentValue / myStats.health.baseValue);
        }
    }
}
