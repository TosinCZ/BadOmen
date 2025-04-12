using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveAndLoad : MonoBehaviour
{
    public GameObject player;
    public EnemySpawner enemySpawner;
    public Character_Sprite charSprite;
    public SimpleRandomWalkDungeonGen dungeonGen;
    public TileMapVisualiser tileMapVisualiser;
    private PlayerIsAttacked _playerAttacked;

    public void Serialize()
    {
        Character_Sprite _charSprite = charSprite.GetComponent<Character_Sprite>();
        SimpleRandomWalkDungeonGen _dungeonGen = dungeonGen.GetComponent<SimpleRandomWalkDungeonGen>();
        HashSet<Vector2Int> floorPositions = dungeonGen.floorPositions;
        List<int> Floorpos = new List<int>();
        foreach (var pos in floorPositions)
        {
            Floorpos.Add(pos.x);
            Floorpos.Add(pos.y);
        }
        SaveGame.Serialize(player, enemySpawner, charSprite, dungeonGen, Floorpos);
    }

    public void LoadPlayer()
    {
        SaveData data = SaveGame.LoadPlayer();
        player.GetComponent<PlayerIsAttacked>().Health = data.Health;
        Debug.Log(data.Health);
        charSprite.GetComponent<Character_Sprite>().Score = data.Score;
        enemySpawner.GetComponent<EnemySpawner>().NumEnemies = data.NumEnemies;
        player.transform.position = new Vector2 (data.PlayerPosition[0],data.PlayerPosition[1]);

        HashSet<Vector2Int> FloorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < data.Floorpos.Count; i += 2)
        {
            Vector2Int pos = new Vector2Int(data.Floorpos[i], data.Floorpos[i + 1]);
            FloorPositions.Add(pos);
        }
        if (tileMapVisualiser != null)
        {
            tileMapVisualiser.Clear();
            // Other tileMapVisualiser method calls
        }
        else
        {
            Debug.LogWarning("tileMapVisualiser is null");
        }

        tileMapVisualiser.PaintFloorTiles(FloorPositions);
        Wall_Generator.CreateWalls(FloorPositions, tileMapVisualiser);
        enemySpawner.SpawnEnemies(FloorPositions);
    }
}
