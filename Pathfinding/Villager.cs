using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

//this is the character that fulfils the task given by the task manager this is being done by moving to the target position
public class Villager : MonoBehaviour{
    public int food;
    public int Water;
    public bool eat;
    public bool drink;

    public float movementSpeed;
    PathfindingGrid.NodePos targetNode;
    public Transform targetTransform;
    int pathInt;

    List<Vector3> pathList = new List<Vector3>();

    void Start(){
        TaskManager.tm.villagerList.Add(this);
    }

    void Update(){
        //here it will check if there is a target if there isn't one it will ask for one
        if(targetTransform != null){
            PathfindingGrid.NodePos _currentTarggetPos = PathfindingGrid.Pathfinder.checkNode(targetTransform.position);
            //this checks if the villager is at the target
            if(targetNode.x != _currentTarggetPos.x || targetNode.y != _currentTarggetPos.y){
                targetNode = _currentTarggetPos;
                pathList = PathfindingGrid.Pathfinder.MakePath(targetTransform.position, transform.position);
                pathInt = 0;
            }
            //this moves the villager to the next point witch will get him closer to his target
            else if(pathInt < pathList.Count){
                Vector3 newpos = Vector3.MoveTowards(transform.position, pathList[pathList.Count - pathInt - 1], movementSpeed * Time.deltaTime);
                transform.position = new Vector3(newpos.x,transform.position.y,newpos.z);
                if(Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(pathList[pathList.Count - pathInt - 1].x, 0, pathList[pathList.Count - pathInt - 1].z)) < 0.1f){
                    if(pathInt == pathList.Count - 1){
                        targetTransform.gameObject.GetComponent<Task>().StartTask(this);
                    }
                    pathInt++;
                }
            }
            else{
                nextObjective();
            }
        }
        else{
            nextObjective();
        }
    }

    //here the villager asks for a new task
    public void nextObjective(){
        targetTransform = null;
        targetTransform = TaskManager.tm.NextTask(this);
    }

    //this makes the villager eat food
    public void Eat(){
        ResourceManager.rm.food--;
        food++;
    }

    //this makes the villager drink water
    public void Drink(){
        ResourceManager.rm.water--;
        Water++;
    }
}
