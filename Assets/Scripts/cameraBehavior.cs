using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBehavior : MonoBehaviour
{

    public Transform player;
    //public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        //get camera to follow player
        transform.position = new Vector3 (player.position.x, player.position.y, transform.position.z);
    }
}
