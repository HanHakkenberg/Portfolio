using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianAI : MonoBehaviour{
    public Transform player;
    public Transform myTransform;
    public NavMeshAgent navigator;
    public List<GameObject> checkPoints = new List<GameObject>();
    public int currentCheckPoint;
    public RaycastHit rayHit;
    public RaycastHit rayHit2;
    public string doorTag;
    public string carTag;
    public string playerTag;
    public string aiTag;
    public AIManager aim;
    public bool stationary;
    public InteractManager im;
    public int angleOfSight;
    public Transform lookDirection;
    public float timer;
    public int stilStandingTime;
    public bool standStil;
    public Animator characterAnimation;
    private bool jaNee;

    void Start() {
        characterAnimation.SetBool("Walking",true);
        navigator.SetDestination(checkPoints[currentCheckPoint].transform.position);
    }

    void Update() {
        if(aim.looking == true) {
            StealNoticeVoid();
        }
        if(Physics.Raycast(transform.position,transform.forward,out rayHit,10) && rayHit.collider.gameObject.tag == doorTag || Physics.Raycast(transform.position,transform.forward,out rayHit,2) && rayHit.collider.gameObject.tag == carTag) {
            if(rayHit.collider.gameObject.tag == doorTag) {
                rayHit.collider.GetComponent<Interactable>().Interacting();
            }
        }
        else if(stationary == false && standStil == false) {
            characterAnimation.SetBool("Walking",true);
            navigator.SetDestination(checkPoints[currentCheckPoint].transform.position);
            if(myTransform.position.z == checkPoints[currentCheckPoint].transform.position.z && myTransform.position.x == checkPoints[currentCheckPoint].transform.position.x) {
                if(currentCheckPoint == checkPoints.Count - 1) {
                    timer = stilStandingTime;
                    standStil = true;
                    characterAnimation.SetBool("Walking",false);
                }
                else {
                    currentCheckPoint++;
                }
            }

        }
        else if(standStil == true) {
            if(timer <= 0) {
                standStil = false;
                currentCheckPoint = 0;
            }
            else {
                timer -= Time.deltaTime;
            }
        }
    }


    public void StealNoticeVoid() {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 toPlayer = player.position - transform.position;
        if(Vector3.Dot(forward,toPlayer) > 0) {
            Vector3 targetDir = player.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir,transform.forward);
            if(angle < angleOfSight) {
                lookDirection.LookAt(player);
                if(Physics.Raycast(transform.position,lookDirection.forward,out rayHit2,8) && rayHit2.collider.tag == "Player") {
                    AIManager.wantedState = true;
                }
            }
        }
    }

    void OnCollisionEnter(Collision c) {

        if(c.collider.tag == aiTag && jaNee == false){
            jaNee = true;
            currentCheckPoint++;
            if(currentCheckPoint == checkPoints.Count - 1){
                currentCheckPoint = 0;
            }
            navigator.SetDestination(checkPoints[currentCheckPoint].transform.position);
        }
    }

    void OnCollisionExit(Collision c){
        if(c.collider.tag == aiTag && jaNee == true) {
            jaNee = false;
        }
    }
}