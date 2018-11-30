using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour{
    public string playerTag;
    public AIManager aim;

    void OnTriggerEnter(Collider c){
        if(c.GetComponent<Collider>().tag == playerTag) {
            aim.looking = true;
        }
    }
}
