using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour {
    public Transform myTransform;
    public NavMeshAgent navigator;
    public List<GameObject> checkPoints = new List<GameObject>();
    public int currentCheckPoint;
    public RaycastHit rayHit;
    public string carTag;
    public string playerTag;
    public int raydistance;
    public float missingAmount;

    void Update() {
        navigator.SetDestination(checkPoints[currentCheckPoint].transform.position);
        Physics.Raycast(transform.position,transform.forward,out rayHit,raydistance);
        if(Physics.Raycast(transform.position,transform.forward,out rayHit,raydistance) && rayHit.collider.tag == carTag){
        }
        else {
            if(myTransform.position.z >= checkPoints[currentCheckPoint].transform.position.z - missingAmount && myTransform.position.z <= checkPoints[currentCheckPoint].transform.position.z + missingAmount && myTransform.position.x >= checkPoints[currentCheckPoint].transform.position.x - missingAmount && myTransform.position.x <= checkPoints[currentCheckPoint].transform.position.x + missingAmount) {
                if(currentCheckPoint == checkPoints.Count - 1) {
                    currentCheckPoint = 0;
                }
                else {
                    currentCheckPoint++;
                }
                navigator.SetDestination(checkPoints[currentCheckPoint].transform.position);
            }
        }
    }

    void OnCollisionEnter(Collision col){
        if(col.collider.tag == playerTag && MenuManager.playing == true){
            print("test");
            MenuManager.dead = true;
        }
    }
}