using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAI : MonoBehaviour{
    public Transform player;
    public int angleOfSight;
    public Transform lookDirection;
    public RaycastHit rayHit;
    public AIManager aim;
    public int turningSpeed;
    public int maxAngle;
    public bool turningSide;
    public float turn;
    
    void Start(){
        turn = transform.rotation.y;
    }

    void Update(){
        if(aim.looking == true){
            StealNoticeVoid();
        }
        if(turningSide == true){
            turn += turningSpeed * Time.deltaTime;
        }
        else{
            turn -= turningSpeed * Time.deltaTime;
        }
        turn = Mathf.Clamp(turn,-maxAngle,maxAngle);
        transform.localEulerAngles = new Vector3(transform.rotation.x,TurningSide(turn),-20);
    }

    public void StealNoticeVoid(){
        print("ja");
        Vector3 forward = transform.right;
        Vector3 toPlayer = (player.position - transform.position).normalized;
        if(Vector3.Dot(forward,toPlayer) > 0){
            print("1");
            float angle = Vector3.Angle(toPlayer,forward);
            if(angle < angleOfSight){
                print("2");
                Debug.DrawRay(transform.position,transform.forward * 8);
                lookDirection.LookAt(player);
                if(Physics.Raycast(lookDirection.position,lookDirection.forward,out rayHit,8) && rayHit.collider.tag == "Player"){
                    print("3");
                    AIManager.wantedState = true;
                    Debug.DrawRay(lookDirection.position,lookDirection.forward * 8);
                }
            }
        }
    }

    float TurningSide(float sideToTurn){
        if(sideToTurn == maxAngle){
            turningSide = false;
        }
        else if(sideToTurn == -maxAngle) {
            turningSide = true;
        }
        return (sideToTurn);
    }
}
