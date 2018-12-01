using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class holds the information of one node on the path finding grid
    [System.Serializable]
public class Node{
    public Vector3 nodeWorldPos;
    public bool walkable;
    public int parentNodeX, parentNodeY;
    public int myNodeX, myNodeY;
    public int aCost, bCost, tCost;
    public bool Check;

    //this sets certain values when the class is made
    public Node(Vector3 _NodeWorldPos, bool _walkable, int _myNodeX, int _myNodeY){
        myNodeX = _myNodeX;
        myNodeY = _myNodeY;
        nodeWorldPos = _NodeWorldPos;
        walkable = _walkable;
    }

    //this changes the costs
    public void ChangeCost(int _aCost, int _bCost){
        aCost = _aCost;
        bCost = _bCost;
        tCost = aCost + bCost;
    }

    //this changes the parrent node of this node
    public void ChangeParent(int _parentNodeX, int _parentNodeY){
        parentNodeX = _parentNodeX;
        parentNodeY = _parentNodeY;
    }
}
