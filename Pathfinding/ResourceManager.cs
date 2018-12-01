using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is the resorce manager he keeps track of all the resorces and the cost to make items
public class ResourceManager : MonoBehaviour{
    public static ResourceManager rm;
    public int wood;
    public int stone;
    public int water;
    public int food;
    public int tools;

    public int toolCostStone;
    public int toolCostWood;

    void Awake(){
        rm = this;
    }

    public bool ToolCost(){
        if(toolCostStone <= stone && toolCostWood <= wood){
            return (true);
        }
        return (false);
    }

    public void MakeTool(){
        wood -= toolCostWood;
        stone -= toolCostStone;
    }
}
