using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import UnityEngine.UI to use Text class (TMP_Text)
using UnityEngine.Events; // For my event triggers
using UnityEngine.SceneManagement;

public class PlayerIsAttacked : MonoBehaviour
{
    [SerializeField]
    public GameObject enemyObject;
    public Rigidbody2D rb;
    public TMP_Text HealthUIComponent;
    public Character_Sprite charSprite;

    private int privHealth = 300;
    public int Health
    {
        get { return privHealth; }
        set
        {
            if(privHealth != value)
            {
                //Here we're only updating the text shown on screen when the value is changed
                privHealth = value;
                UpdateHealthText(privHealth);
            }
        }
    }

    void Awake()
    {
        // Check if a text component is assigned
        if (HealthUIComponent == null)
        {
            Debug.Log("Health Text component not assigned");
            this.enabled = false; // Disable the script if no Text component is assigned
            return;
        }
    
        // Display the initial health value
        UpdateHealthText(Health);   
    }

    public void UpdateHealthText (int value) 
    {      
        HealthUIComponent.text =  value.ToString(); //Updates the health ui by setting the `text` variable
    }
    public void Attack() // Pass health by reference
    {
        Health -= 10; // Update health 
        if (Health <= 0){
            PlayerPrefs.SetInt("Score_", charSprite.Score);
            PlayerPrefs.Save();
            SceneManager.LoadScene("DeathScreen");
        }
    }
 
}



