using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParameters_" ,menuName = "PCG/SimpleRandomWalkData")]
public class SRW_SO : ScriptableObject //allows us to create this through the inspector
{
    public int iterations = 10, walkLength = 10;
    public bool startRandomlyEachIteration = true;
    
}
