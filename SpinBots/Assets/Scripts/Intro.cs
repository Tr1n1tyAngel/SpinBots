using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Input.GetKeyDown(KeyCode.O) && SceneManager.GetActiveScene().name=="PVP")
        {
            gameManager.p2Ready = true;
        }
        
    }
}
