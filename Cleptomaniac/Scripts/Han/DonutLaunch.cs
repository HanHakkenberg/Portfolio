using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutLaunch : MonoBehaviour{
    public GameObject donut;
    public GameObject shootDonut;
    public SecurityAI sai;
    public UpgradeMenu ugm;

    void Update(){
        if(Input.GetButtonDown("Throw") && sai.toDoOrToDonut()){
            if(ugm.donutAmount > 0){
                ugm.donutAmount -= 1;
                shootDonut = Instantiate(donut);
                shootDonut.transform.position = transform.position;
                shootDonut.transform.rotation = transform.rotation;
            }

        }
    }
}
