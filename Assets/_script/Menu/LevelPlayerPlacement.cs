using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelPlayerPlacement : MonoBehaviour
{
    public Transform PlayerObject;


    // Start is called before the first frame update

    public void PlacePlayer(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> floorPositionsList = floorPositions.ToList();
        int RandomIndex = Random.Range(0,floorPositions.Count);
        Vector2Int SpawnPos = floorPositionsList[RandomIndex];
        IEnumerable<Vector2Int> EdgePositions = Wall_Generator.FindEdgeTiles(floorPositions, Direction2D.DirectionList);
        while(EdgePositions.Contains(SpawnPos)){
            RandomIndex = Random.Range(0,floorPositions.Count);
            SpawnPos = floorPositionsList[RandomIndex];
        }
        PlayerObject.position = new Vector3Int(SpawnPos.x, SpawnPos.y, 0);
    }
}
