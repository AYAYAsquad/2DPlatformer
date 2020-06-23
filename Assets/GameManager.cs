using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private CharacterController2D thePlayer;
    private Vector3 playerStartPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerStartPoint = thePlayer.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
