using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGrid : MonoBehaviour {

    public Transform StartPosition;  //This is where the program will start the pathfinding from.
    public LayerMask WallMask;  //This is the mask that the program will look for when trying to find obstructions to the path.
    public Vector2 vGridWorldSize;  //A vector2 to store the width and height of the graph in world units.
    public float fNodeRadius;  //This stores how big each square on the graph will be
    public float fDistanceBetweenNodes;  //The distance that the squares will spawn from eachother.

    private Vector2 center = new Vector2();
    private Vector2 bottomLeft = new Vector2();

    Node[,] NodeArray;  //The array of nodes that the A Star algorithm uses.
    public List<Node> FinalPath;  //The completed path that the red line will be drawn along

    float fNodeDiameter;  //Twice the amount of the radius (Set in the start function)
    int iGridSizeX, iGridSizeY;  //Size of the Grid in Array units.

    //Ran once the program starts
    private void Start() {

        fNodeDiameter = fNodeRadius * 2;  //Double the radius to get diameter
        iGridSizeX = Mathf.RoundToInt(vGridWorldSize.x / fNodeDiameter);  //Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        iGridSizeY = Mathf.RoundToInt(vGridWorldSize.y / fNodeDiameter);  //Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        CreateGrid();  //Draw the grid
    }

    private void Update() {
        NodeArray = null;
        
    }

    void CreateGrid() {
        NodeArray = new Node[iGridSizeX, iGridSizeY];  //Declare the array of nodes.
        //Vector3 bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;  //Get the real world position of the bottom left of the grid.

        center.x = StartPosition.position.x; // + vGridWorldSize.x / 4;
        center.y = StartPosition.position.y + vGridWorldSize.y / 2;
        bottomLeft.x = center.x - vGridWorldSize.x / 2; // transform.position.x - vGridWorldSize.x / 2;
        bottomLeft.y = center.y - vGridWorldSize.y / 2; // transform.position.y + vGridWorldSize.y / 2;

        for (int x = 0; x < iGridSizeX; x++) {  //Loop through the array of nodes.
            for (int y = 0; y < iGridSizeY; y++) {  //Loop through the array of nodes
                //Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);//Get the world co ordinates of the bottom left of the graph
                Vector2 worldPoint = new Vector2();
                worldPoint.x = bottomLeft.x + x * fNodeDiameter + fNodeRadius;
                worldPoint.y = bottomLeft.y + y * fNodeDiameter + fNodeRadius;

                //print(worldPoint);

                bool Wall = false;

                //If the node is not being obstructed
                //Quick collision check against the current node and anything in the world at its position. If it is colliding with an object with a WallMask,
                //The if statement will return false.
                //if (Physics.CheckSphere(worldPoint, fNodeRadius, WallMask)) {
                //RaycastHit2D castRes = Physics2D.CircleCast(worldPoint, fNodeRadius, new Vector2(0.0f, 0.0f), 0, WallMask);
                if (Physics2D.OverlapCircle(worldPoint, fNodeRadius, WallMask)) {
                    print("Overlap!");
                    Wall = true;  //Object is not a wall
                }

                NodeArray[x, y] = new Node(Wall, worldPoint, x, y);  //Create a new node in the array.
            }
        }
    }

    //Function that gets the neighboring nodes of the given node.
    public List<Node> GetNeighboringNodes(Node a_NeighborNode) {
        List<Node> NeighborList = new List<Node>();  //Make a new list of all available neighbors.
        int icheckX;  //Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int icheckY;  //Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        //Check the right side of the current node.
        icheckX = a_NeighborNode.iGridX + 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < iGridSizeX) {  //If the XPosition is in range of the array
            if (icheckY >= 0 && icheckY < iGridSizeY) {   //If the YPosition is in range of the array
                NeighborList.Add(NodeArray[icheckX, icheckY]);  //Add the grid to the available neighbors list
            }
        }
        //Check the Left side of the current node.
        icheckX = a_NeighborNode.iGridX - 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < iGridSizeX) {  //If the XPosition is in range of the array
            if (icheckY >= 0 && icheckY < iGridSizeY) {  //If the YPosition is in range of the array
                NeighborList.Add(NodeArray[icheckX, icheckY]);  //Add the grid to the available neighbors list
            }
        }
        //Check the Top side of the current node.
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY + 1;
        if (icheckX >= 0 && icheckX < iGridSizeX) {  //If the XPosition is in range of the array
            if (icheckY >= 0 && icheckY < iGridSizeY) {  //If the YPosition is in range of the array
                NeighborList.Add(NodeArray[icheckX, icheckY]);  //Add the grid to the available neighbors list
            }
        }
        //Check the Bottom side of the current node.
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY - 1;
        if (icheckX >= 0 && icheckX < iGridSizeX) {  //If the XPosition is in range of the array
            if (icheckY >= 0 && icheckY < iGridSizeY) {  //If the YPosition is in range of the array
                NeighborList.Add(NodeArray[icheckX, icheckY]);  //Add the grid to the available neighbors list
            }
        }

        return NeighborList;  //Return the neighbors list.
    }

    //Gets the closest node to the given world position.
    public Node NodeFromWorldPoint(Vector3 a_vWorldPos) {
        float ixPos = ((a_vWorldPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x);
        float iyPos = ((a_vWorldPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);

        return NodeArray[ix, iy];
    }

    
    //Function that draws the wireframe
    /*
    private void OnDrawGizmos() {

        Quaternion rot = transform.rotation; // * Quaternion.Euler(90, 0, 0);
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, rot, transform.lossyScale);

        Gizmos.matrix = rotationMatrix;
        //Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, vGridWorldSize.y, 0));  //Draw a wire cube with the given dimensions from the Unity inspector
        Gizmos.DrawWireCube(center, new Vector3(vGridWorldSize.x, vGridWorldSize.y, 0));  //Draw a wire cube with the given dimensions from the Unity inspector


        if (NodeArray != null) {  //If the grid is not empty
            foreach (Node n in NodeArray) {  //Loop through every node in the grid
                if (n.bIsWall) {  //If the current node is a wall node
                    Gizmos.color = Color.white;  //Set the color of the node
                } else {
                    Gizmos.color = Color.yellow;  //Set the color of the node
                }


                if (FinalPath != null) {  //If the final path is not empty
                    if (FinalPath.Contains(n)) {  //If the current node is in the final path
                        Gizmos.color = Color.red;  //Set the color of that node
                    }

                }

                //Vector3 worldPos = new Vector3(n.vPosition.x, n.vPosition.y, 0);
                //Gizmos.DrawCube(n.vPosition, Vector3.one * (fNodeDiameter - fDistanceBetweenNodes));  //Draw the node at the position of the node.
                //Gizmos.DrawCube(worldPos, Vector3.one * (fNodeDiameter - fDistanceBetweenNodes));
                Gizmos.DrawWireCube(n.vPosition, new Vector3(fNodeDiameter, fNodeDiameter, 0));
            }
        }
    }
    */
    
}