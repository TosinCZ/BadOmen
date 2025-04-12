using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TileMapVisualiser : MonoBehaviour
{
    [SerializeField]
    public Tilemap floortilemap; // All the variables under the SerializeField can be assigned values and changed in the inspector
    [SerializeField]              // The Tilemap variables are a grid object each one being at a different Z value
    private Tilemap walltilemap;  // The TileBase arrays hold the tile textures
    [SerializeField]
    private Tilemap walltilemap2; 
    [SerializeField]
    private Tilemap walltilemap3;
    [SerializeField]
    private Tilemap walltilemap4;
    [SerializeField]
    private TileBase[] floorTile; 
    [SerializeField]
    private TileBase[] WallTile;


    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        foreach (var position in floorPositions)
        {
            PaintSingleFloorTile(floortilemap, GetRandomFloorTile(), position); // For each tile position the function GetRandomTile randomly chooses a texture and
        }                                                                       // PaintSingleFloorTile applies that texture to the tile
    }

    private TileBase GetRandomFloorTile() // Returns a random Floor tile
    {
        int randomIndex = Random.Range(0, floorTile.Length); // Chooses a random number in the range of the array
        return floorTile[randomIndex]; // Returns the tile in the randomly chosen index
    }
 
    public void IdentifyWallType(Vector2Int position, List<Vector2Int> LocationEmptyNeighbour, HashSet<Vector2Int> floorPositions) // For each tile at the edge the subroutine figures out where 
    {                                                                                                                              // walls should be placed and what type of wall should go there
        List<Tilemap> ListOfTileMap = new List<Tilemap> {walltilemap, walltilemap2, walltilemap3, walltilemap4};
        
        if (position == null || LocationEmptyNeighbour == null || floorPositions == null)  // If not all the parameters are given this will stop the subroutine preventing an error 
        {
            Debug.LogError("One or more input parameters are null.");
            return;
        }

        if (ListOfTileMap.Count != 4 || WallTile.Length == 0)   // If the Tilemap list is empty or if no wall tile textures are assigned this will stop the subroutine
        {
            Debug.LogError("Invalid ListOfTileMap or WallTile array.");
            return;
        }

        
        foreach (var direction in Direction2D.DirectionList) // Checks all 4 cardinal directions of floor tiles at the edge and adds the empty locations to a list called LocationEmptyNeighbour
        {
            Vector2Int neighbourpos = position + direction;
            if (floorPositions.Contains(neighbourpos) != true)
            {
                LocationEmptyNeighbour.Add(neighbourpos);
            }
        }
        if (LocationEmptyNeighbour.Count() == 3) //For edge tiles with 3 empty neighbours this if statement figures out the location of the empty tiles diagonal to it
        {
            List<Vector2Int> HorizontalEmptyTiles = new List<Vector2Int>(); 
            List<Vector2Int> VerticleEmptyTiles = new List<Vector2Int>();         
            foreach (var item in LocationEmptyNeighbour) // splits the empty neighbours into two groups based on if they are horizontal or verticle to the tile
            {
                Vector2Int location = item - position;
                if (location.x == 0)
                {
                    VerticleEmptyTiles.Add(item);
                }
                else if (location.y == 0)
                {
                    HorizontalEmptyTiles.Add(item);
                }
            }
            
            foreach (var Hlocation in HorizontalEmptyTiles) // Uses known empty neighbouring tiles to work out the corner positions and adds the location to the list
            {
                foreach (var Vlocation in VerticleEmptyTiles)
                {
                    Vector2Int Addeddirection =(Hlocation - position) + (Vlocation - position);
                    Vector2Int CornerLocation = position + Addeddirection;
                    if (!floorPositions.Contains(CornerLocation))
                    {
                        LocationEmptyNeighbour.Add(CornerLocation);
                    }
                }
            }
        }
        else if (LocationEmptyNeighbour.Count() == 2) // Find the diagonal empty tile of a edge tile with 2 empty neighbours
        {
            Vector2Int CheckCornerLocation = LocationEmptyNeighbour[0] + LocationEmptyNeighbour[1];
            if (CheckCornerLocation.x != 0 && CheckCornerLocation.y != 0)
            {   
                Vector2Int Addeddirection =(LocationEmptyNeighbour[0] - position) +(LocationEmptyNeighbour[1] - position);
                Vector2Int CornerLocation = position + Addeddirection;   
                if (!Addeddirection.Equals(Vector2Int.zero) && !floorPositions.Contains(CornerLocation) && !LocationEmptyNeighbour.Contains(CornerLocation))          
                {
                    LocationEmptyNeighbour.Add(CornerLocation);
                }
            }
        }
        

        if (LocationEmptyNeighbour.Count > 0)
        {
            List<int> TileIndexNumbers = AssignAdjacentWallTexture(position, LocationEmptyNeighbour,floorPositions, ListOfTileMap, WallTile); // assigns each empty wall tile an index number
            bool TileCheck;
            for (int i = 0; i <= (LocationEmptyNeighbour.Count-1); i++)
            {
                for (int x = 0; x < 4; x++) // this for loop will cycle through the tilemaps on different z values until it finds one with an empty tile at the position needed
                {
                    int WallTileNum = TileIndexNumbers[i];
                    TileCheck = IsTileEmpty(LocationEmptyNeighbour[i], ListOfTileMap[x], x);
                    if (TileCheck == true)
                    {
                        PaintSingleWallType(ListOfTileMap[x], WallTile[WallTileNum], LocationEmptyNeighbour[i], LocationEmptyNeighbour,x);                    
                        break; // Exits out of the x for loop once a space has been found and painted
                    } 
                }             
            }
            TileIndexNumbers.Clear();
        }
        LocationEmptyNeighbour.Clear(); 
    }




    List<int> AssignAdjacentWallTexture(Vector2Int position, List<Vector2Int> LocationEmptyNeighbour, HashSet<Vector2Int> floorPositions, List<Tilemap> ListOfTileMap, TileBase[] WallTile) 
    {
        List<int> TileIndexNumbers = new List<int>(); // This function assigns each wall tile an number that correlates to the index location of the correct texture based on the location of the empty tile in comparison with the edge tiles


        foreach (var location in LocationEmptyNeighbour)
        {
            Vector2Int DirectionNeighbour = position - location;
            
            if (DirectionNeighbour.x == 0 && DirectionNeighbour.y == 1) //bottom
            {
                TileIndexNumbers.Add(0);
            }
            else if (DirectionNeighbour.x == 0 && DirectionNeighbour.y == -1) //up
            {
                TileIndexNumbers.Add(1);
            }
            else if (DirectionNeighbour.x == -1 && DirectionNeighbour.y ==0) //right
            {
                TileIndexNumbers.Add(2);
            }
            else if (DirectionNeighbour.x == 1 && DirectionNeighbour.y == 0) //left
            {
                TileIndexNumbers.Add(3);
            }
            else if (DirectionNeighbour.x == -1 && DirectionNeighbour.y == 1) // Bottom Right
            {
                TileIndexNumbers.Add(4);
            }
            else if (DirectionNeighbour.x == 1 && DirectionNeighbour.y == 1) // Bottom Left
            {
                TileIndexNumbers.Add(5);
            }
            else if (DirectionNeighbour.x == -1 && DirectionNeighbour.y == -1) // Top Right
            {
                TileIndexNumbers.Add(6);
            }
            else if (DirectionNeighbour.x == 1 && DirectionNeighbour.y == -1) // Top Left
            {
                TileIndexNumbers.Add(7);
            } 
            else
            {
                Debug.LogError("Error calculating corner tiles"+DirectionNeighbour);
            }           
        }


        return TileIndexNumbers;
    }
    
    void PaintSingleWallType(Tilemap tilemap, TileBase tile, Vector2Int position, List<Vector2Int> LocationEmptyNeighbour,int x) // Paints the floor tile given as a parameter
    {

        Vector3Int TilePosition = new Vector3Int(position.x, position.y, x);
        tilemap.SetTile(TilePosition, tile);
        
    }

    void PaintSingleFloorTile(Tilemap tilemap, TileBase tile, Vector2Int position) // Paints the wall tile
    {
        Vector3Int TilePosition = new Vector3Int(position.x, position.y, 0);
        tilemap.SetTile(TilePosition, tile);
        
    }

    public bool IsTileEmpty(Vector2Int position, Tilemap tilemap, int x) // Checks if the tile position passed through is empty
    {
        Vector3Int TilePosition = new Vector3Int(position.x, position.y, x);
        TileBase tile = tilemap.GetTile(TilePosition);
        if (tile == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Clear() // When the map is regenerated this subroutine is used to reset all the tilemaps
    {
       floortilemap.ClearAllTiles();
       walltilemap.ClearAllTiles();
       walltilemap2.ClearAllTiles();
       walltilemap3.ClearAllTiles();
       walltilemap4.ClearAllTiles();
    }    
}
