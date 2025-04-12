using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.Tilemaps;

public class Enemy_Radius : MonoBehaviour
{
    private bool PlayerInsideRadius = false;
    private Animator Animate;
    private Vector2Int EnemyIntPos;
    private Stack<Vector2Int> ShortestPath;
    private bool isMovingTowardsTile; 
    private Vector2Int PlayerIntPos;
    private AStarAlgorithm AStar;
    private bool IsAttacking;
    private bool ReRunPathfinder;
    public SimpleRandomWalkDungeonGen SRW;
    public TileMapVisualiser TMV;
    [SerializeField]
    private Transform Omie;

    [SerializeField]
    public float radius = 30f;

    [SerializeField]
    public float moveSpeed = 5f;



    void Start()
    {
        AStar = GetComponent<AStarAlgorithm>();
        Animate = GetComponent<Animator>();
    }
  
    
    IEnumerator ExecuteEnemyHit(float duration)
    {  
        GameObject attakedObject = GameObject.FindGameObjectWithTag("Omie"); // Finds the object with the tage Omie which is the player object
        if (attakedObject == null)
        {
            Debug.LogError("No game object with tag 'Omie' found!");
            yield break; // Exit the coroutine early if no game object with the tag is found
        }
        PlayerIsAttacked playerIsAttacked = attakedObject.GetComponent<PlayerIsAttacked>(); // Gets the script Attacking that's attached to the obhect
        if (playerIsAttacked == null)
        {
            Debug.LogError("No script!");
            yield break; // Exit the coroutine early if the component isn’t found
        }
        IsAttacking = true;
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        Animate.SetBool("Attacking", true); // Set Attacking to true
        playerIsAttacked.Attack(); // This method reduces the health variable
        IsAttacking = false;

    }


    void Update()
    {
        Tilemap floortilemap = TMV.floortilemap;
        float DistanceFromPlayer = Vector2.Distance(transform.position, Omie.position);
        if (isMovingTowardsTile == false)
            if (DistanceFromPlayer <= 1 && !IsAttacking)
            {
                ReRunPathfinder = true;
                Animate.SetBool("IsMoving",false);
                StartCoroutine(ExecuteEnemyHit(1.0f));
            }
            if(DistanceFromPlayer <= radius && DistanceFromPlayer>1)
            {
                Animate.SetBool("Attacking",false);//The attacking animation is set to false as the enemy is not close enough to attack
                if (!PlayerInsideRadius) 
                {
                    PlayerInsideRadius = true;
                }

                Vector2 rayDirection = (Omie.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, radius);
                Debug.DrawRay(transform.position, rayDirection * radius, Color.green); 

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == Omie.gameObject)
                    {
                    ReRunPathfinder = true;
                    Animate.SetBool("IsMoving",true);
                    MoveTowardsPlayer(rayDirection);
                    // I have this variable to know when the slime is moving for the animator. I don't use the PlayerInsideRadius variable because it is true when the enemy isn't moving because of an obstruction
                    }
                    

                    else if (hit.collider.gameObject.tag == "Wall"){//If the player is behind a wall this next section occurs
                        SimpleRandomWalkDungeonGen _dungeonGen = SRW.GetComponent<SimpleRandomWalkDungeonGen>();
                        HashSet<Vector2Int> floorPositions = _dungeonGen.floorPositions;

                        if (ReRunPathfinder == true){
                            PlayerIntPos =  Vector2Int.RoundToInt(Omie.position);
                            Debug.Log(PlayerIntPos);
                            EnemyIntPos = Vector2Int.RoundToInt(transform.position);       
                            ShortestPath = AStar.FindPath( EnemyIntPos, PlayerIntPos, floorPositions); 
                            ReRunPathfinder = false;
                            while (!ReRunPathfinder && ShortestPath != null){
                                if(ShortestPath.Count == 0){
                                    Debug.Log("End");
                                    ReRunPathfinder = true;
                                    break;
                                }
                                if (ReRunPathfinder){
                                    break;
                                }
                                Vector2Int nextTile = ShortestPath.Pop();
                                Vector3 nextPosition = new Vector3(nextTile.x, nextTile.y, transform.position.z);
                                Vector3Int TilePosition = floortilemap.WorldToCell(nextPosition);
                                if (Vector3.Distance(transform.position, TilePosition) <= 0.1f){
                                    Debug.Log("Moved to " + TilePosition);
                                    
                                }
                                transform.position = Vector3.MoveTowards(transform.position, TilePosition, 1f * Time.deltaTime);
                            }
                        }

                    }

                    else {
                        Animate.SetBool("IsMoving",false);
                    }
                    
                }
            }   

            else if (DistanceFromPlayer > radius)
            {
                if (PlayerInsideRadius)
                {
                    PlayerInsideRadius = false;
                    Animate.SetBool("Attacking",false);
                    Animate.SetBool("IsMoving",false);
                }
            }
        }
    

    protected void MoveTowardsPlayer(Vector2 rayDirection)
    {
        Vector3 direction = rayDirection;
        RaycastHit hitX, hitY; 
        Debug.Log("Chasing");
        

        if (Physics.Raycast(transform.position, new Vector3(direction.x, 0f, 0f), out hitX, Mathf.Abs(direction.x) * moveSpeed * Time.deltaTime) && hitX.collider.gameObject.name != "Omie") // Checks for potential collisions
        {
            direction.x = 0f; // Prevent horizontal movement if a collision is detected, 
        }

        // Vertical movement
        if (Physics.Raycast(transform.position, new Vector3(0f, direction.y, 0f), out hitY, Mathf.Abs(direction.y) * moveSpeed * Time.deltaTime) && hitY.collider.gameObject.name != "Omie")
        {
            direction.y = 0f; // Prevent vertical movement if a collision is detected
        }
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}