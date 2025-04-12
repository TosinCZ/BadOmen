using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Wall_Generator 
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisualiser TileMapVisualiser)
    {
        IEnumerable<Vector2Int> EdgePositions = FindEdgeTiles(floorPositions, Direction2D.DirectionList);
        List<Vector2Int> LocationEmptyNeighbour = new List<Vector2Int>();
        foreach (var position in EdgePositions) // Loops through each floortile that is on the edge to pass through to the IndentifyWallType function
        {
            TileMapVisualiser.IdentifyWallType(position, LocationEmptyNeighbour, floorPositions); 
        }
    }

    public static IEnumerable<Vector2Int> FindEdgeTiles(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionlist) // Adds all tiles with at least one empty neighbour to the EdgePosition list
    {
        HashSet<Vector2Int> EdgePositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions) 
        {
            bool hasEmptyNeighbour = false;
            foreach (var direction in directionlist)
            {
                var neighbourpos = position + direction;
                if (!floorPositions.Contains(neighbourpos))
                {
                    hasEmptyNeighbour = true;
                }
            }
            if (hasEmptyNeighbour)
            {
                EdgePositions.Add(position);
            }
        }
        
        return EdgePositions;
    }



}
