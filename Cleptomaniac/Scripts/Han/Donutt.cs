using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donutt : MonoBehaviour {
    public float timer;
    public int atractTime;
    public SecurityAI sai;
    public int donutSpeed;

    void Start() {
        timer = atractTime;
        sai = new SecurityAI();
        GetComponent<Rigidbody>().AddForce (transform.right * donutSpeed);
        sai.Distracted(gameObject);
    }

    void Update() {
        timer -= Time.deltaTime;
        if(timer <= 0) {
            sai.Distracted(null);
            Destroy(gameObject);
        }
    }
}
