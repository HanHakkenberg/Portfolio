using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagebles : MonoBehaviour {
    public Stats myStats;

    public virtual void TakeDamage(float damage){
        myStats.health.currentValue -= damage;
    }
}
