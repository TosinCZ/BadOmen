using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class AbstractDungeonGen : MonoBehaviour
{
    [SerializeField]
    protected TileMapVisualiser TileMapVisualiser = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;
    
    public void GenerateDungeon()
    {
        TileMapVisualiser.Clear();
        DestroyAllClones();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();

    public void DestroyAllClones()
    {

        GameObject[] clones = GameObject.FindGameObjectsWithTag("Clone");

        // Iterate through each clone and destroy them
        foreach (GameObject clone in clones)
        {
            Destroy(clone);
        }
    }
    
}
