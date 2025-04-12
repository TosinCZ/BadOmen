using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGeneration 
{
    public static HashSet <Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength) 
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>(); //path is what is returned
        
        path.Add(startPosition);  // hashset adds the star position
        var previousposition = startPosition; //starts as the same value

        for (int i = 0; i < walkLength; i++) //step loop for agent, agent can move up until it reaches its walk length
        {
            var newposition = previousposition + Direction2D.GetRandomCardinalDirection(); //move one space in a random direction
            path.Add(newposition);
            previousposition = newposition;
        }
        return path; //returns the path of the simple random walk
    }

    public static List <Vector2Int> GenerateCorridor(Vector2Int CorridorStartPos, int walkLength, HashSet<Vector2Int> floorPositions) 
    {
        Vector2Int position = CorridorStartPos;
        List<Vector2Int> CorridorPath = new List<Vector2Int>();
        CorridorPath.Add(CorridorStartPos);

        var RandomDirection = Direction2D.GetRandomCardinalDirection();
        int loops = 0;

        for (int i = 0; i <= walkLength; i++) 
        {
            while (floorPositions.Contains(position + RandomDirection))
            {
                loops += 1;
                if (loops == 1)
                {
                    RandomDirection = RandomDirection * -1;
                }
                else if(loops == 2)
                {
                    RandomDirection = new Vector2Int(RandomDirection.y, RandomDirection.x);
                }
                else if(loops == 3)
                {
                    RandomDirection = RandomDirection * -1;
                }
                else
                {
                    break;
                }
                //RandomDirection = Direction2D.GetRandomCardinalDirection();
            }            
            position += RandomDirection;
            CorridorPath.Add(position);
        }
        
        
        return CorridorPath; 
    }
}

public static class Direction2D //class to choose a random direction
{
    public static List<Vector2Int> DirectionList = new List<Vector2Int> //everything in the section below is in the list
    {
        new Vector2Int (0,1),  //up
        new Vector2Int (1,0),  //Right
        new Vector2Int (0,-1), //Down
        new Vector2Int (-1,0), //left
    };
    public static Vector2Int GetRandomCardinalDirection()
    {
        return DirectionList[Random.Range(0 , DirectionList.Count)];
    }
}