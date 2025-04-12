using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Character_Sprite : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f; // The speed of the character
    public Rigidbody2D rb;
    public Animator animator;
    private bool QPressed = false;
    private Vector2 IdleDirection = new Vector2 (0,-1);
    private LayerMask layerToIgnore;
    private LayerMask EnemyLayer;
    public TMP_Text ScoreUIComponent;
    private int privScore = 0;
    

    void Awake(){
        // Check if a text component is assigned
        if (ScoreUIComponent == null || animator == null || rb == null)
        {
            Debug.Log("Score Text or Animator or RigidBody2D component not assigned");
            this.enabled = false; //Disable the script if no Text component is assigned
            return;
        }
        UpdateScoreText(Score);
        // Display the initial health value
    }    

    void UpdateScoreText (int value) 
    {      
        ScoreUIComponent.text = "Score: " + value.ToString(); //Updates the health ui by setting the `text` variable
    }
    
    void Start(){
        EnemyLayer = LayerMask.NameToLayer("Enemy"); // Assign the correct layer to EnemyLayer

        layerToIgnore = 1 << LayerMask.NameToLayer("Omie"); // Assign the correct layer to layerToIgnore

    }

    void Update()
    {
        Movement(); // Evey frame the Movement method is called
    }

    
    public int Score 
    {
        get { return privScore; }
        set
        {
            if(privScore != value)
            {
                //Here we're only updating the text shown on screen when the value is changed
                privScore = value;
                UpdateScoreText(privScore);
            }
        }
    }

    void Movement()    
    {    
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        float HMovement = Input.GetAxis("Horizontal"); // Gives a value between -1 and 1 based on the users arrow input
        float VMovement = Input.GetAxis("Vertical");
        Vector2 Movement = new Vector2(HMovement, VMovement); 
        Movement.Normalize(); // Usually diagonal movement is faster due to it counting as 2 inputs in one, this prevents that
        if (Movement != new Vector2(0,0))
        {
            IdleDirection = Movement;
        }
        RaycastHit hitX, hitY; // A ray cast is sent in the direction of movement and checks if it intercepts an object with a collider 

        if (Physics.Raycast(transform.position, new Vector3(Movement.x, 0f, 0f), out hitX, Mathf.Abs(Movement.x) * moveSpeed * Time.deltaTime)) // Checks for potential collisions
        {
            Movement.x = 0f; // Prevent horizontal movement if a collision is detected, 
        }

        // Vertical movement
        if (Physics.Raycast(transform.position, new Vector3(0f, Movement.y, 0f), out hitY, Mathf.Abs(Movement.y) * moveSpeed * Time.deltaTime))
        {
            Movement.y = 0f; // Prevent vertical movement if a collision is detected
        }
        rb.velocity = new Vector2(Movement.x * moveSpeed, Movement.y * moveSpeed);// Moves the rigidbody component
        //transform.position =  transform.positions + HMovement * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q) && !QPressed){
            QPressed = true;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, IdleDirection, 4, ~layerToIgnore);
            if (hit.collider != null) 
            {
                GameObject TargetedObject = hit.collider.gameObject;
                if (TargetedObject.name == "Enemy" || TargetedObject.name == "Enemy(Clone)"){
                    TargetedObject.GetComponent<EnemyRecieveDmg>().DealDamage(TargetedObject);
                }
            }
        }
        
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            QPressed = false;
        }

        if(SceneManager.GetActiveScene().name == "BadOmen" && Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

   



}
