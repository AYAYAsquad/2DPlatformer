using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    
    public CharacterController2D Player1;
    private Vector3 playerStartPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerStartPoint = Player1.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame() {
        StartCoroutine("RestartGameCo");
    }

    public IEnumerator RestartGameCo() {
        yield return new WaitForSeconds(0.01f);
        Player1.transform.position = playerStartPoint;
    }
}
