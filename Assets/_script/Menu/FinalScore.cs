using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FinalScore : MonoBehaviour
{
    public TMP_Text ScoreTextComponent; // Reference to the Text component in the Unity Editor

    void Start()
    {     
        // Update the text of the Text component to display the value of the score variable
        int Score = PlayerPrefs.GetInt("Score_");
        ScoreTextComponent.text = "Score: " + Score.ToString();
    }


}
