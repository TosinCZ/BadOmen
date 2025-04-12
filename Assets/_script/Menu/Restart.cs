using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public Character_Sprite charSprite;
    public PlayerIsAttacked playerIsAttacked;
    public LevelManager levelManager;
    
    public void NewGame()
    {
        charSprite.Score = 0;
        playerIsAttacked.Health = 300;
        levelManager.changeScene();

    }

}
