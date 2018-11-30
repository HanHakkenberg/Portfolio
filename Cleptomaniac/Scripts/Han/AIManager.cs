using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour{
    public static bool wantedState;
    public bool looking;
    public float timer;
    public SecurityAI sai;
    public AudioSource themeSong;
    public float wantedSpeed;
    public float normalSpeed;
    public float speedAddOrRemove;

    void Start(){
        sai = new SecurityAI();
        themeSong.pitch = normalSpeed;
    }


    void Update(){
        if(looking && timer < 0){
            looking = false;
        }
        else if(timer > 0){
            timer -= Time.deltaTime;
        }
        if(!wantedState && themeSong.pitch > normalSpeed){
            themeSong.pitch -= speedAddOrRemove;
        }
        else if(themeSong.pitch < wantedSpeed){
            themeSong.pitch += speedAddOrRemove;
        }
    }
}
