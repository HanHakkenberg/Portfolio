using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Soldier : Damagebles {
    public NavMeshAgent agent;
    public float attackCooldown;
    public Transform targetTransform;
    public Animator anim;
    public bool inFight;
    public AudioSource myAudiosource;

    void Start() {
        MyStart();
    }

    public virtual void MyStart() {
        if(agent != null) {
            agent.speed = Random.Range(1.75f, 2.25f);
        }
        myAudiosource.pitch = Random.Range(0.75f, 1.25f);
        myAudiosource.volume = Random.Range(0.01f, 0.08f);
        transform.localScale *= Random.Range(0.9f, 1.1f);
    }

    public override void TakeDamage(float damage) {
    }
}
