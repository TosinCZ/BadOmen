using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int Health;
    public float[] PlayerPosition;
    public int Score;
    public int NumEnemies;
    public List<int> Floorpos;


    public SaveData(GameObject player, EnemySpawner enemySpawner, Character_Sprite charSprite, SimpleRandomWalkDungeonGen dungeonGen, List<int> floorPositions){

        Health = player.GetComponent<PlayerIsAttacked>().Health;
        PlayerPosition = new float[2];
        PlayerPosition[0] = player.transform.position.x;
        PlayerPosition[1] = player.transform.position.y;
        Score = charSprite.Score;
        NumEnemies = enemySpawner.totalEnemies;
        Floorpos = floorPositions;
    }
}
