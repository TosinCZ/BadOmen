using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGen : AbstractDungeonGen
{

   public Vector2Int Startpos;
   public HashSet<Vector2Int> floorPositions { get; private set; }
   public SaveAndLoad saveAndLoad;
   [SerializeField] //reveals the contents of the file below to the inspector
   private SRW_SO randomWalkParameters; 

   [SerializeField]
   private int SetNumRooms;

   [SerializeField]
   private EnemySpawner enemySpawner;
   
   [SerializeField]
   private LevelPlayerPlacement levelPlayerPlacement;

   

   //iterations - the amount of iterations random walk will run for
   //walkLength - passed to the random walk script to dictate the amount of steps taken 
   //startRandomlyEachIteration -   allows us to start from any position from the path not just the same start position. this is important as we can join procedurally generated 
                                  //sections together by setting the start to a section of the previous
   
   void Start(){
      if(PlayerPrefs.HasKey("Loading")){
         if(PlayerPrefs.GetInt("Loading") == 1)
         {
            Debug.Log("SRW LOADING = 1");
            saveAndLoad.LoadPlayer();
            PlayerPrefs.SetInt("Loading", 0);
         }
         else{
            GenerateDungeon();
         }
      }
      else{
      GenerateDungeon();
      }
   }

   public void NewMap(){
      Debug.Log("NewMap");
      RunProceduralGeneration();
   }

   protected override void RunProceduralGeneration()
   {
      // Generate corridors and rooms
      floorPositions = new HashSet<Vector2Int>();
      int NumRooms = SetNumRooms;
      HashSet<Vector2Int> RoomResult = RunRandomWalk();  // Starts the RunRandomWalk procedure
      floorPositions.UnionWith(RoomResult);


      ;
      if (TileMapVisualiser != null) 
      {
         TileMapVisualiser.PaintFloorTiles(floorPositions); // Calls the procedure that will paint the floor tiles
      }
      
      else // Error handles if there is no value assigned to the TileMapVisualiser
      {
         Debug.LogError("TileMapVisualiser is not assigned!");
      }
      Wall_Generator.CreateWalls(floorPositions, TileMapVisualiser);
      levelPlayerPlacement.PlacePlayer(floorPositions);
      enemySpawner.SpawnEnemies(floorPositions);
   }

   protected HashSet<Vector2Int> RunRandomWalk()
   {
      var currentPosition = Startpos;  //current postion = 0
      HashSet<Vector2Int> RandomWalkPositions = new HashSet<Vector2Int>();
      for (int i = 0; i < randomWalkParameters.iterations ; i++) // One Iteration does one simple walk 
      {
         var path = ProceduralGeneration.SimpleRandomWalk(currentPosition, randomWalkParameters.walkLength); // the path from the function is returned
         RandomWalkPositions.UnionWith(path); // UnionWith adds the positions from the path to floor positions while removing duplicates
         if(randomWalkParameters.startRandomlyEachIteration)
            currentPosition = RandomWalkPositions.ElementAt(Random.Range(0, RandomWalkPositions.Count)); //randomly chooses a point in the path 
      }                                                                                       //to make the new start 
      
      return RandomWalkPositions;
   }
}
