using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : MonoBehaviour
{
    private HashSet<Vector2Int> walkableTiles;
    private float Heuristic;    

    public Stack<Vector2Int> FindPath(Vector2Int startPos, Vector2Int targetPos, HashSet<Vector2Int> walkableTiles)
    {
        this.walkableTiles = walkableTiles;
        List<Vector2Int> UnexploredTiles = new List<Vector2Int>(); // unexplored floor tiles
        HashSet<Vector2Int> ExploredTiles = new HashSet<Vector2Int>(); // floor tiles that have been fully explored
        Dictionary<Vector2Int, Vector2Int> NextBestStep = new Dictionary<Vector2Int, Vector2Int>();//Holds the best adjacent tile to come from to get to the tile in the key quickly
        Dictionary<Vector2Int, float> GCost = new Dictionary<Vector2Int, float>();//Holds the cost to get from the start position to the tile in the key position
        Dictionary<Vector2Int, float> FCost = new Dictionary<Vector2Int, float>();//Holds the estimated cost to get from the start position to the target position when going through the current tile



        UnexploredTiles.Add(startPos);
        GCost[startPos] = 0;
        FCost[startPos] = HeuristicCostEstimate(startPos, targetPos); //A heuristical estimate of how long it will take to get from the start position to the end position 

        while (UnexploredTiles.Count > 0) // While there are still tiles to explore
        {
            Vector2Int current = GetLowestFCost(UnexploredTiles, FCost); //finds a tile in UnexploredTiles with the lowest f score

            if (current == targetPos) //If the algorithm has reached target position the optimal path is reconstructed
            {
                return ReconstructPath(NextBestStep, current); 
            }

            UnexploredTiles.Remove(current); //Moves the current tile from the unexplored tiles list to the explored tiles list
            ExploredTiles.Add(current);

            foreach (Vector2Int neighbor in GetNeighbors(current))//Goes through every neighbour of the current tile that is part of the walkable tiles hashset
            {
                if (ExploredTiles.Contains(neighbor) || !walkableTiles.Contains(neighbor)) //If the neighbour is not a floor tile or is in the ExploredTiles then the algorithm goes to the next neighbour
                    continue;

                float tempGCost = GCost[current] + Vector2Int.Distance(current, neighbor);//Calculates the Cost of getting from the start position to the neighbour position through the player position
                if (!UnexploredTiles.Contains(neighbor) || tempGCost < GCost[neighbor]) //Checks if the neighbour tile isn't already in UnexploredTiles or if the cost of getting to the neighbour tile through the current tile is less than the previous cost calculated for the tile
                {
                    NextBestStep[neighbor] = current;//Updates the NextBestStep dictionary so that the best tile to come in from to get to the neighbour tile is the current tile
                    GCost[neighbor] = tempGCost; //The GCost is replaced with the new shortest cost
                    FCost[neighbor] = GCost[neighbor] + HeuristicCostEstimate(neighbor, targetPos); //The estimated shorted cost it would take to get from the enemy to the player through the neighbour tile is calculated and added to the dictionary

                    if (!UnexploredTiles.Contains(neighbor))//If the neighbour tile isn't in the UnexploredTiles it is added so that it can be eventually visited
                        UnexploredTiles.Add(neighbor);
                }
            }
        }
        return null; //If no path can be found
    }

    float HeuristicCostEstimate(Vector2Int start, Vector2Int target) //Calculates the shortest distance from the neigbour position to the player, this value is used to estimate the distance it will take to complete the path from the current tile to the player
    {
        Heuristic = (Mathf.Abs(start.x - target.x) + Mathf.Abs(start.y - target.y));
        return Heuristic; 
    }

    Vector2Int GetLowestFCost(List<Vector2Int> UnexploredTiles, Dictionary<Vector2Int, float> FCost) //Finds the tile where the path from the enemy to the player is shortest when it runs through it
    {
        Vector2Int lowest = UnexploredTiles[0];
        foreach (Vector2Int node in UnexploredTiles)
        {
            if (FCost.ContainsKey(node) && FCost[node] < FCost[lowest]) // If the next tile has a calculated f score and if that value is smaller than the f score of the lowest fscore node then that tile becomes the new lowest
            {
                lowest = node;
            }
        }
        return lowest;
    }

    List<Vector2Int> GetNeighbors(Vector2Int position) //This method finds all the adjacent tiles that are walkable floor tiles
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] directions =  //Adjacent directions
        {
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
            new Vector2Int(1,0),
            new Vector2Int(-1,0)
        };

        foreach (Vector2Int direction in directions) //All adjacent walkable neighbouring tiles is added to the neighbouring tile
        {
            Vector2Int neighbor = position + direction;
            if (walkableTiles.Contains(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    Stack<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> NextBestStep, Vector2Int current) //Reconstructs the shortest path
    {
        Stack<Vector2Int> path = new Stack<Vector2Int>();
        path.Push(current); //The positions are pushed into a stack so that it can be reversed to the correct order
        while (NextBestStep.ContainsKey(current))
        {
            current = NextBestStep[current];
            path.Push(current);
        }
        return path;
    }
}
