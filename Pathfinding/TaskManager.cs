using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script gives a villager a new task when he asks for one
public class TaskManager : MonoBehaviour{
    public static TaskManager tm;
    public Transform mine, well, smith;
    public List<Transform> berries = new List<Transform>();
    public List<Transform> trees = new List<Transform>();
    public List<Transform> sticks = new List<Transform>();
    public List<Transform> rocks = new List<Transform>();
    public List<Villager> villagerList = new List<Villager>();
    public int foodSeconds;

    void Awake(){
        tm = this;
        StartCoroutine(healthOff());
    }

    public Transform NextTask(Villager _villager){
        if (_villager.food <= 30 || _villager.eat){
            if (_villager.food <= 30){
                _villager.eat = true;
            }
            else if (_villager.food >= 100){
                _villager.eat = false;
            }
            if (ResourceManager.rm.food == 0){
                return (berries[Random.Range(0, berries.Count)]);
            }
            else{
                _villager.Eat();
                return (NextTask(_villager));
            }
        }
        if (_villager.Water <= 30 || _villager.drink){
            if(_villager.Water <= 30){
                _villager.drink = true;
            }
            else if(_villager.Water >= 100){
                _villager.drink = false;
            }
            if (ResourceManager.rm.water == 0){
                return (well);
            }
            else{
                _villager.Drink();
                return (NextTask(_villager));
            }
        }
        if(ResourceManager.rm.ToolCost()){
            return(smith);
        }
        if(ResourceManager.rm.wood < ResourceManager.rm.stone){
            if(ResourceManager.rm.tools > 0){
                return(trees[Random.Range(0,trees.Count)]);
            }
            return (sticks[Random.Range(0, sticks.Count)]);
        }
        else if(ResourceManager.rm.tools > 0){
            return (mine);
        }
        else{
            return (rocks[Random.Range(0, rocks.Count)]);
        }
    }

    //this keeps taking off food and water after the given amount of seconds
    IEnumerator healthOff(){
        yield return new WaitForSeconds(foodSeconds);
        StartCoroutine(healthOff());
        for (int i = 0; i < villagerList.Count; i++){
            villagerList[i].food--;
            villagerList[i].Water -= 2;
        }
    }
}
