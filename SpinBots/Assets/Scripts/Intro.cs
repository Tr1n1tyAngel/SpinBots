using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            gameManager.p1Ready = true;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            gameManager.p2Ready = true;
        }
        
    }
}
