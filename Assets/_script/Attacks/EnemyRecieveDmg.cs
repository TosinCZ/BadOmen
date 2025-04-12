using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class EnemyRecieveDmg : MonoBehaviour
{
    int EnemyHealth = 100;
    public float xForceToAdd;
    public float yForceToAdd;
    public Rigidbody2D rigidForForce;
    private bool CurrentlyDying;
    private Animator animator;
    public UnityEvent OnBegin, OnEnd; 
    public Character_Sprite charactersprite;
    public EnemySpawner enemySpawner;

    public RaycastHit2D hit; 

    private bool KnockbackFlag = false;
    float xDir;
    float yDir;



    void Start()
    {
        rigidForForce = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    IEnumerator PlayDeathAnimationAndWait()
    {
        animator.SetTrigger("Dead");
        // Wait for the animation to finish playing
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null; // Wait for the next frame
        }
        charactersprite.Score += 20;
        
        // Activate the animation trigger
        if(gameObject.tag == "Enemy"){
            Debug.Log("original deactivated");
            gameObject.SetActive(false);
        }
        else{
            Destroy(gameObject);
        }
        CurrentlyDying = false;
        enemySpawner.CheckIfAllEnemiesDead();    
    }

    void Update(){
        if (EnemyHealth <= 0 && !CurrentlyDying){
            CurrentlyDying = true;
            StartCoroutine(PlayDeathAnimationAndWait());
        }
    }

    public void DealDamage (GameObject TargetEnemy){
        if (!CurrentlyDying){
            animator.SetTrigger("Hurt");
            EnemyHealth -= 25;
            Knockback(TargetEnemy);
        }
    }

    public void Knockback(GameObject other)
    {
        OnBegin?.Invoke();
        Vector2 initialHitPoint = transform.position;

        // Calculate the direction of the knockback based on the hit point relative to this object's position
        float xDir = Mathf.Sign(other.transform.position.x - initialHitPoint.x );
        float yDir = Mathf.Sign(other.transform.position.y - initialHitPoint.y);
        Vector2 VelocityDir = new Vector2(xDir,yDir);

        // If the raycast hits a collider, stop the object
        // Apply knockback force
        KnockbackFlag = true;
        rigidForForce.velocity = new Vector2(xDir * xForceToAdd, yDir * yForceToAdd); 

    }

    const float deceleration = 2f; // Adjust this value to control deceleration rate   

    void FixedUpdate()
    {
        // Apply deceleration to gradually slow down the object over time
        Vector2 currentVelocity = rigidForForce.velocity;
        currentVelocity -= currentVelocity.normalized * deceleration * Time.deltaTime;
        
        // Ensure that deceleration doesn't reverse direction
        if (Vector2.Dot(currentVelocity, rigidForForce.velocity) < 0f)
        {
            currentVelocity = Vector2.zero; // Stop the object if deceleration changes direction
            KnockbackFlag = false;
            OnEnd?.Invoke();
        }

        if (KnockbackFlag)
        {
            Vector2 VelocityDir = new Vector2(xDir,yDir);
            hit = Physics2D.Raycast(transform.position, VelocityDir, 10);
            Debug.DrawRay(transform.position, VelocityDir * 10, Color.blue); 
            if (hit.collider != null && hit.collider.tag == "Wall"){
                currentVelocity = Vector2.zero;
                KnockbackFlag = false;
                OnEnd?.Invoke();
            }
        }
        rigidForForce.velocity = currentVelocity;
    }
}
