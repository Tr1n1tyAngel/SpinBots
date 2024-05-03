using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSelection : MonoBehaviour
{
    public GameManager gameManager;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && gameManager.p1ChargeSelected == false)
        {
            gameManager.p1ChargeSelected = true;
            gameManager.p1ShieldSelected = false;
            gameManager.p1SpeedBoostSelected = false;
            gameManager.p1PowerupSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.S) && gameManager.p1ShieldSelected == false)
        {
            gameManager.p1ChargeSelected = false;
            gameManager.p1ShieldSelected = true;
            gameManager.p1SpeedBoostSelected = false;
            gameManager.p1PowerupSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.D) && gameManager.p1SpeedBoostSelected == false)
        {
            gameManager.p1ChargeSelected = false;
            gameManager.p1ShieldSelected = false;
            gameManager.p1SpeedBoostSelected = true;
            gameManager.p1PowerupSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.J) && gameManager.p2ChargeSelected == false)
        {
            gameManager.p2ChargeSelected = true;
            gameManager.p2ShieldSelected = false;
            gameManager.p2SpeedBoostSelected = false;
            gameManager.p2PowerupSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.K) && gameManager.p2ShieldSelected == false)
        {
            gameManager.p2ChargeSelected = false;
            gameManager.p2ShieldSelected = true;
            gameManager.p2SpeedBoostSelected = false;
            gameManager.p2PowerupSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.L) && gameManager.p2SpeedBoostSelected == false)
        {
            gameManager.p2ChargeSelected = false;
            gameManager.p2ShieldSelected = false;
            gameManager.p2SpeedBoostSelected = true;
            gameManager.p2PowerupSelected = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && gameManager.p1PowerupSelected)
        {
            gameManager.p1Ready = true;
            gameManager.powP1Ready.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.O) && gameManager.p2PowerupSelected)
        {
            gameManager.p2Ready = true;
            gameManager.powP2Ready.SetActive(true);
        }
    }
}
