using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleSelection : MonoBehaviour
{
   public GameManager gameManager;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && gameManager.p1CubeSelected == false)
        {
            gameManager.p1CubeSelected = true;
            gameManager.p1CylinderSelected = false;
            gameManager.p1SphereSelected = false;
            gameManager.p1ObstacleSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.S) && gameManager.p1CylinderSelected == false)
        {
            gameManager.p1CubeSelected = false;
            gameManager.p1CylinderSelected = true;
            gameManager.p1SphereSelected = false;
            gameManager.p1ObstacleSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.D) && gameManager.p1SphereSelected == false)
        {
            gameManager.p1CubeSelected = false;
            gameManager.p1CylinderSelected = false;
            gameManager.p1SphereSelected = true;
            gameManager.p1ObstacleSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && gameManager.p1ObstacleSelected)
        {
            gameManager.p1Ready = true;
            gameManager.obsP1Ready.SetActive(true);
        }

        if(SceneManager.GetActiveScene().name == "PVP")
        {
            if (Input.GetKeyDown(KeyCode.J) && gameManager.p2CubeSelected == false)
            {
                gameManager.p2CubeSelected = true;
                gameManager.p2CylinderSelected = false;
                gameManager.p2SphereSelected = false;
                gameManager.p2ObstacleSelected = true;
            }
            if (Input.GetKeyDown(KeyCode.K) && gameManager.p2CylinderSelected == false)
            {
                gameManager.p2CubeSelected = false;
                gameManager.p2CylinderSelected = true;
                gameManager.p2SphereSelected = false;
                gameManager.p2ObstacleSelected = true;
            }
            if (Input.GetKeyDown(KeyCode.L) && gameManager.p2SphereSelected == false)
            {
                gameManager.p2CubeSelected = false;
                gameManager.p2CylinderSelected = false;
                gameManager.p2SphereSelected = true;
                gameManager.p2ObstacleSelected = true;
            }

            if (Input.GetKeyDown(KeyCode.O) && gameManager.p2ObstacleSelected)
            {
                gameManager.p2Ready = true;
                gameManager.obsP2Ready.SetActive(true);
            }
        }
        else
        {
            int rnd = Random.Range(0, 3);
            switch(rnd)
            {
                case 0:
                    gameManager.p2CubeSelected = true;
                    gameManager.p2CylinderSelected = false;
                    gameManager.p2SphereSelected = false;
                    gameManager.p2ObstacleSelected = true;
                    break;
                case 1:
                    gameManager.p2CubeSelected = false;
                    gameManager.p2CylinderSelected = true;
                    gameManager.p2SphereSelected = false;
                    gameManager.p2ObstacleSelected = true;
                    break;
                case 2:
                    gameManager.p2CubeSelected = false;
                    gameManager.p2CylinderSelected = false;
                    gameManager.p2SphereSelected = true;
                    gameManager.p2ObstacleSelected = true;
                    break;
            }
            gameManager.p2Ready = true;
            gameManager.obsP2Ready.SetActive(true);
        }
        
    }
}
