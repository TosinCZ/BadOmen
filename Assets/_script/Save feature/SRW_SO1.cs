using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParameters2_" ,menuName = "PCG/SimpleRandomWalkData2")]
public class SRW_SO1 : ScriptableObject //allows us to create this through the inspector
{
    public int iterations = 100, walkLength = 100;
    public bool startRandomlyEachIteration = true;
    
}
