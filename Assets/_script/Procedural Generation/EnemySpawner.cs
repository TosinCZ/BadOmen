using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyObject;
    public int NumEnemies;
    public TileMapVisualiser tileMapVisualiser;
    public int totalEnemies;
    public Transform PlayerTransform; 
    public SimpleRandomWalkDungeonGen dungeonGenerator;
    private Vector2Int SpawnPos;
    private int RandomIndex;

    // Start is called before the first frame update
    public void SpawnEnemies(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> floorPositionsList = floorPositions.ToList();
        EnemyObject.SetActive(true);
        Debug.Log("Enemy object is active");
        IEnumerable<Vector2Int> EdgePositions = Wall_Generator.FindEdgeTiles(floorPositions, Direction2D.DirectionList);
        for  (int i = 0; i < NumEnemies-1; i++)
        {
            RandomIndex = Random.Range(0,floorPositions.Count);
            SpawnPos = floorPositionsList[RandomIndex];
            while(EdgePositions.Contains(SpawnPos)){
                RandomIndex = Random.Range(0,floorPositions.Count);
                SpawnPos = floorPositionsList[RandomIndex];
            }
            GameObject enemy = Instantiate(EnemyObject, new Vector3Int(SpawnPos.x, SpawnPos.y, 0), Quaternion.identity);
            enemy.tag = "Clone";
            int LayerIndex = LayerMask.NameToLayer("Ignore Raycast");
            enemy.layer = LayerIndex;
        }

        int _RandomIndex = Random.Range(0,floorPositions.Count);
        Vector2Int _SpawnPos = floorPositionsList[_RandomIndex];
        while(EdgePositions.Contains(SpawnPos)){
                RandomIndex = Random.Range(0,floorPositions.Count);
                SpawnPos = floorPositionsList[RandomIndex];
            }
        PlayerTransform.position = new Vector3Int(_SpawnPos.x, _SpawnPos.y, 0);
        totalEnemies = GameObject.FindGameObjectsWithTag("Clone").Length + 1;
    }

    public void CheckIfAllEnemiesDead(){
        totalEnemies -= 1;
        if (totalEnemies <= 0 ){
            tileMapVisualiser.Clear();
            dungeonGenerator.NewMap();
        }
    }


}
