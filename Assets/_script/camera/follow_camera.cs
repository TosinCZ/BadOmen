using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform PlayerNode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new  Vector3(PlayerNode.transform.position.x, PlayerNode.transform.position.y, transform.position.z);
    }
}
