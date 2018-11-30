using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecurityAI : MonoBehaviour{
    public Transform player;
    public Transform myTransform;
    public NavMeshAgent navigator;
    public List<GameObject> checkPoints = new List<GameObject>();
    public int currentCheckPoint;
    public RaycastHit rayHit;
    public string doorTag;
    public string carTag;
    public AIManager aim;
    public bool stationary;
    public Animator securityAnimator;
    public InteractManager im;
    public int fieldOfView;
    public Transform character;
    public int angleOfSight;
    public Transform lookDirection;
    public static GameObject donut;
    public float runningSpeed;
    public float walkingSpeed;
    public Transform startingPosition;

    void Update() {
        if(aim.looking == true) {
            StealNoticeVoid();
        }
        if(Physics.Raycast(transform.position,transform.forward,out rayHit,10) && rayHit.collider.gameObject.tag == doorTag || Physics.Raycast(transform.position,transform.forward,out rayHit,2) && rayHit.collider.gameObject.tag == carTag) {
            rayHit.collider.GetComponent<Interactable>().Interacting();
        }
        else if(AIManager.wantedState == false) {
            if(stationary == false) {
                securityAnimator.SetBool("Walking",true);
                securityAnimator.SetBool("Wanted",false);
                navigator.speed = walkingSpeed;
                navigator.SetDestination(checkPoints[currentCheckPoint].transform.position);
                if(myTransform.position.z == checkPoints[currentCheckPoint].transform.position.z && myTransform.position.x == checkPoints[currentCheckPoint].transform.position.x) {
                    if(currentCheckPoint == checkPoints.Count - 1) {
                        currentCheckPoint = 0;
                    }
                    else {
                        currentCheckPoint++;
                    }
                }
            }
            else {
                transform.rotation = startingPosition.rotation;
                transform.position = startingPosition.position;
                securityAnimator.SetBool("Walking",false);
                securityAnimator.SetBool("Wanted",false);
                if(Random.Range(1,100) == 2) {
                    securityAnimator.SetBool("Tip That Hat",true);
                    securityAnimator.SetBool("Tip That Hat",false);
                }
            }
        }
        else {
            if(donut != null) {
                navigator.SetDestination(donut.transform.position);
                if(myTransform.position.z == donut.transform.position.z && myTransform.position.x == donut.transform.position.x) {
                    securityAnimator.SetBool("Walking",false);
                    securityAnimator.SetBool("Wanted",false);
                }
            }
            else {
                navigator.speed = runningSpeed;
                securityAnimator.SetBool("Wanted",true);
                navigator.SetDestination(player.position);
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
                if(Physics.Raycast(transform.position,lookDirection.forward,out rayHit,8) && rayHit.collider.tag == "Player") {
                    AIManager.wantedState = true;
                }
            }
        }


    }

    public void OnCollisionEnter(Collision collision) {
        if(AIManager.wantedState == true && collision.collider.tag == "Player") {
            MenuManager.dead = true;
            securityAnimator.SetBool("Wanted",false);
            aim.looking = false;
        }
    }

    public void Distracted(GameObject getDonut) {
        donut = getDonut;
    }

    public bool toDoOrToDonut() {
        if(donut != null) {
            return (false);
        }
        else {
            return (true);
        }
    }
}

