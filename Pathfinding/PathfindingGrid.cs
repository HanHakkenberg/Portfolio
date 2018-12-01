using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour{
    public static PathfindingGrid Pathfinder;
    Node[,] grid;
    public int gridSizeX,gridSizeY;
    public float nodeSize;
    List<NodePos> openList = new List<NodePos>();
    List<NodePos> gizmoCloseList = new List<NodePos>(); //Remove
    List<Vector3> gizmoPathList = new List<Vector3>();  //Remove
    NodePos targetNode, myNode;
    public int maxTravelDistance;

	void Awake(){
        CreatGrid();
	}

    //Creating the grid
    void CreatGrid() {
        Pathfinder = this;
        grid = new Node[gridSizeX, gridSizeY];
        for(int x = 0; x < gridSizeX; x++){
            for(int y = 0; y < gridSizeY; y++){
                Vector3 newNodePos = new Vector3(transform.position.x + (x * nodeSize) + nodeSize / 2,0, transform.position.z + (y * nodeSize) + nodeSize / 2);
                grid[x, y] = new Node(newNodePos, !Physics.CheckSphere(newNodePos, nodeSize / 2,2) ,x,y);
            }
        }
    }

    //converting the given node to a nodepos
    public NodePos checkNode(Vector3 _nodeToCheck){
        NodePos newPos = new NodePos(Mathf.Clamp(Mathf.RoundToInt(((_nodeToCheck.x - transform.position.x) - nodeSize / 2) / nodeSize), 1, gridSizeX - 1), Mathf.Clamp(Mathf.RoundToInt(((_nodeToCheck.z - transform.position.z) - nodeSize / 2) / nodeSize),1,gridSizeY - 1));
        if(!grid[newPos.x,newPos.y].walkable){
            List<NodePos> checkList = new List<NodePos>();
            List<NodePos> doneList = new List<NodePos>();
            checkList.Add(newPos);
            while (checkList.Count > 0){
                for (int x = -1; x < 2; x++){
                    for (int y = -1; y < 2; y++){
                        if(!(x == 0 && y == 0) && checkList[0].x + x > 0 && checkList[0].x + x < gridSizeX && checkList[0].y + y > 0 && checkList[0].y + y < gridSizeY && !grid[checkList[0].x + x, checkList[0].y + y].Check){
                            grid[checkList[0].x + x, checkList[0].y + y].Check = true;
                            checkList.Add(new NodePos(checkList[0].x + x, checkList[0].y + y));
                            if (grid[checkList[0].x + x, checkList[0].y + y].walkable){
                                foreach (NodePos v in checkList){
                                    grid[v.x, v.y].Check = false;
                                }
                                foreach (NodePos n in doneList){
                                    grid[n.x, n.y].Check = false;
                                }
                                return (new NodePos(checkList[0].x + x, checkList[0].y + y));
                            }
                        }
                    }
                }
                doneList.Add(checkList[0]);
                checkList.RemoveAt(0);
            }
        }
        return (newPos);
    }

    //Creating the path
    public List<Vector3> MakePath(Vector3 _targetPos, Vector3 _myPos){
        targetNode.ChangeNodePos(checkNode(_targetPos).x, checkNode(_targetPos).y);
        myNode.ChangeNodePos(checkNode(_myPos).x, checkNode(_myPos).y);

        //Searching for path to the target
        FindNeighbors(myNode);
        List<NodePos> closeList = new List<NodePos>();
        while (openList.Count > 0){
            int lowest = 0;
            for (int i = 0; i < openList.Count; i++){
                if (grid[openList[i].x, openList[i].y].tCost < grid[openList[lowest].x, openList[lowest].y].tCost){
                    lowest = i;
                }
            }
            closeList.Add(openList[lowest]);
            if (FindNeighbors(openList[lowest])){
                break;
            }
            openList.RemoveAt(lowest);
        }

        //Backtracking most efficient path
        List<Vector3> pathList = new List<Vector3>();
        NodePos nodeToAdd = closeList[closeList.Count - 1];
        int l = 0;
        pathList.Add(grid[targetNode.x, targetNode.y].nodeWorldPos);
        while (l < maxTravelDistance){
            l++;
            if (myNode.x == nodeToAdd.x && myNode.y == nodeToAdd.y){
                gizmoPathList = pathList;   //Remove
                gizmoCloseList = closeList; //Remove
                foreach (NodePos v in closeList){
                    grid[v.x, v.y].Check = false;
                }
                foreach (NodePos n in openList){
                    grid[n.x, n.y].Check = false;
                }
                openList.Clear();
                return (pathList);
            }
            pathList.Add(grid[nodeToAdd.x, nodeToAdd.y].nodeWorldPos);
            nodeToAdd = new NodePos(grid[nodeToAdd.x, nodeToAdd.y].parentNodeX, grid[nodeToAdd.x, nodeToAdd.y].parentNodeY);
        }
        return (new List<Vector3>());
    }

    //Checking the notes around the submitted node
    bool FindNeighbors(NodePos _currentPos){
        for (int x = -1; x < 2; x++){
            for (int y = -1; y < 2; y++){
                if(!(x == 0 && y == 0) && _currentPos.x + x > 0 && _currentPos.x + x < gridSizeX && _currentPos.y + y > 0 && _currentPos.y + y < gridSizeY){
                    Node currentNeighbor = grid[_currentPos.x + x, _currentPos.y + y];
                    if(currentNeighbor.walkable){
                        if(x == 0 || y == 0){
                            currentNeighbor.ChangeCost(grid[_currentPos.x, _currentPos.y].aCost + 10, GetNewCost(Mathf.Abs(currentNeighbor.myNodeX - targetNode.x), Mathf.Abs(currentNeighbor.myNodeY - targetNode.y)));
                        }
                        else{
                            currentNeighbor.ChangeCost(grid[_currentPos.x, _currentPos.y].aCost + 14, GetNewCost(Mathf.Abs(currentNeighbor.myNodeX - targetNode.x), Mathf.Abs(currentNeighbor.myNodeY - targetNode.y)));
                        }
                        currentNeighbor.ChangeCost(grid[_currentPos.x, _currentPos.y].aCost + GetNewCost(Mathf.Abs(currentNeighbor.myNodeX - _currentPos.x), Mathf.Abs(currentNeighbor.myNodeY - _currentPos.y)), GetNewCost(Mathf.Abs(currentNeighbor.myNodeX - targetNode.x), Mathf.Abs(currentNeighbor.myNodeY - targetNode.y)));
                        if (!currentNeighbor.Check){
                            currentNeighbor.ChangeParent(_currentPos.x, _currentPos.y);
                            currentNeighbor.Check = true;
                            grid[_currentPos.x + x, _currentPos.y + y] = currentNeighbor;
                            openList.Add(new NodePos(_currentPos.x + x, _currentPos.y + y));
                        }
                        else if(currentNeighbor.aCost < grid[_currentPos.x + x, _currentPos.y + y].aCost)
                        {
                            currentNeighbor.ChangeParent(_currentPos.x, _currentPos.y);
                            grid[_currentPos.x + x, _currentPos.y + y] = currentNeighbor;
                        }
                        if (currentNeighbor.myNodeX == targetNode.x && currentNeighbor.myNodeY == targetNode.y){
                            return (true);
                        }
                    }
                }
            }
        }
        return (false);
    }

    //calculating cost
    int GetNewCost(int x, int y){
        int cost;

        if (x < y){
            cost = (x * 14) + ((y - x) * 10);
        }
        else{
            cost = (y * 14) + ((x - y) * 10);
        }
        return (cost);
    }

    //Cheap way of storing nodes
        [System.Serializable]
    public struct NodePos{
        public int x, y;

        public NodePos(int _x, int _y){
            x = _x;
            y = _y;
        }

        public void ChangeNodePos(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    //Visualizing the grid and path
    void OnDrawGizmos(){
        Gizmos.DrawWireCube(new Vector3(transform.position.x + (gridSizeX * nodeSize) / 2, 0,transform.position.z + (gridSizeY * nodeSize) / 2), new Vector3(gridSizeX * nodeSize, 1, gridSizeY * nodeSize));
        if (grid != null){
            Gizmos.DrawCube(grid[targetNode.x,targetNode.y].nodeWorldPos, new Vector3(nodeSize - 0.1f, nodeSize - 0.1f, nodeSize - 0.1f));
            Gizmos.DrawCube(grid[myNode.x, myNode.y].nodeWorldPos, new Vector3(nodeSize - 0.1f, nodeSize - 0.1f, nodeSize - 0.1f));

            foreach (NodePos n in gizmoCloseList){
                Gizmos.DrawCube(grid[n.x, n.y].nodeWorldPos, new Vector3(nodeSize - 0.3f, nodeSize - 0.3f, nodeSize - 0.3f));
            }

            foreach (Vector3 n in gizmoPathList)
            {
                Gizmos.DrawCube(n, new Vector3(nodeSize - 0.1f, nodeSize - 0.1f, nodeSize - 0.1f));
            }
        }

    }
}
