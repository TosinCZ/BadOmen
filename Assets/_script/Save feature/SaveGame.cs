using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveGame
{
    public static void Serialize(GameObject player, EnemySpawner enemySpawner, Character_Sprite charSprite, SimpleRandomWalkDungeonGen dungeonGen, List<int> floorPositions){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/BadOmen.vcb";
        FileStream stream = new FileStream(path, FileMode.Create);
        SaveData data = new SaveData(player, enemySpawner, charSprite,dungeonGen, floorPositions);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadPlayer(){
        string path =  Application.persistentDataPath + "/BadOmen.vcb";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found");
            return null;
        }
    }
}
